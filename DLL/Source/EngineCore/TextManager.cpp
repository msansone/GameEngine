#include "..\..\Headers\EngineCore\TextManager.hpp"

using namespace firemelon;
using namespace boost::python;

TextManager::TextManager(RendererPtr renderer, InputDeviceManagerPtr inputDeviceManager, FontManagerPtr fontManager, DebuggerPtr debugger)
{
	debugger_ = debugger;
	renderer_ = renderer;
	inputDeviceManager_ = inputDeviceManager;
	fontManager_ = fontManager;

	inputChannel_ = 0;
}

TextManager::~TextManager()
{
}

void TextManager::cleanup()
{
	inputDeviceManager_->buttonDownSignal_.disconnect(boost::bind(&TextManager::buttonDown, this, _1));
	inputDeviceManager_->buttonUpSignal_.disconnect(boost::bind(&TextManager::buttonUp, this, _1));
	inputDeviceManager_->changeChannelOfListenersSignal_.disconnect(boost::bind(&TextManager::changeInputChannel, this, _1, _2));

	int size = textList_.size();
	for (int i = 0; i < size; i++)
	{
		int size2 = textList_[i].size();
		for (int j = 0; j < size2; j++)
		{
			textList_[i][j]->cleanup();
		}

		textList_[i].clear();
	}

	textList_.clear();

	layerPositions_.clear();
}

void TextManager::initialize()
{	
	inputDeviceManager_->buttonDownSignal_.connect(boost::bind(&TextManager::buttonDown, this, _1));
	inputDeviceManager_->buttonUpSignal_.connect(boost::bind(&TextManager::buttonUp, this, _1));	
	inputDeviceManager_->changeChannelOfListenersSignal_.connect(boost::bind(&TextManager::changeInputChannel, this, _1, _2));
}

void TextManager::preUpdate(int roomIndex)
{
	int roomCount = textList_.size();

	if (roomIndex < roomCount)
	{
		int size = textList_[roomIndex].size();
		for (int i = 0; i < size; i++)
		{	
			textList_[roomIndex][i]->previousX_ = textList_[roomIndex][i]->x_;
			textList_[roomIndex][i]->previousY_ = textList_[roomIndex][i]->y_;
		}
	}
}

void TextManager::update(int roomIndex, double time)
{
	int roomCount = textList_.size();

	if (roomIndex < roomCount)
	{
		boost::shared_ptr<InputDeviceContainer> inputDeviceContainer = inputDeviceManager_->getInputDeviceContainer();
		boost::shared_ptr<InputDeviceWrapper> inputDeviceWrapper = inputDeviceContainer->getInputDevice(inputChannel_);
		boost::shared_ptr<InputDevice> inputDevice = inputDeviceWrapper->getDevice();

		std::vector<int> removalQueue;

		// Update each text element currently in the vector.

		// For entries that suspend the game updates, wait for a keypress to remove them.

		// For entries that do not suspend game updates, update the timer and if it has 
		// exceeded it's display time, remove it. If the display time is zero, it should
		// not be removed.

		int size = textList_[roomIndex].size();
		for (int i = 0; i < size; i++)
		{	
			if (textList_[roomIndex][i]->isRemoved_ == true)
			{
				// This text has exceeded it's display time. Remove it.
				removalQueue.push_back(i);
			}
			else
			{
				textList_[roomIndex][i]->preUpdate(time);
			}
		}

		size = removalQueue.size();
		for (int i = size - 1; i >= 0; i--)
		{
			int indexToDelete = removalQueue[i];
		
			int textListSize = textList_[roomIndex].size();

			if (removalQueue[i] < textListSize)
			{
				textList_[roomIndex].erase(textList_[roomIndex].begin() + indexToDelete);
			}
		}
	}
}

void TextManager::render(int roomIndex, double lerp)
{
	int roomCount = textList_.size();

	if (roomIndex < roomCount)
	{
		// Render all the current text
		int size = textList_[roomIndex].size();
		for (int i = 0; i < size; i++)
		{
			int renderAtX = 0;
			int renderAtY = 0;

			if (textList_[roomIndex][i]->interpolatePosition_ == true)
			{
				int previousX = textList_[roomIndex][i]->previousX_;
				int previousY = textList_[roomIndex][i]->previousY_;
							
				int currentX = textList_[roomIndex][i]->x_;
				int currentY = textList_[roomIndex][i]->y_;
							
				int textPositionX = previousX + (lerp * (currentX - previousX));
				int textPositionY = previousY + (lerp * (currentY - previousY));
				
				int previousLayerX = layerPositions_[roomIndex][textList_[roomIndex][i]->layerIndex_]->getPreviousX();
				int previousLayerY = layerPositions_[roomIndex][textList_[roomIndex][i]->layerIndex_]->getPreviousY();
							
				int currentLayerX = layerPositions_[roomIndex][textList_[roomIndex][i]->layerIndex_]->getX();
				int currentLayerY = layerPositions_[roomIndex][textList_[roomIndex][i]->layerIndex_]->getY();
							
				//int layerPositionX = previousLayerX + (lerp * (currentLayerX - previousLayerX));
				//int layerPositionY = previousLayerY + (lerp * (currentLayerY - previousLayerY));
					
				int layerPositionX = currentLayerX;
				int layerPositionY = currentLayerY;
					
				renderAtX = layerPositionX + textPositionX;
				renderAtY = layerPositionY + textPositionY;
			}
			else
			{
				renderAtX = textList_[roomIndex][i]->x_;
				renderAtY = textList_[roomIndex][i]->y_;

				int layerIndex = textList_[roomIndex][i]->layerIndex_;
				int layerCount = layerPositions_.size();

				if (layerIndex > -1 && layerIndex < layerCount)
				{
					renderAtX += layerPositions_[roomIndex][layerIndex]->getX();
					renderAtY += layerPositions_[roomIndex][layerIndex]->getY();
				}
			}

			textList_[roomIndex][i]->render(renderAtX, renderAtY);
		}
	}
}

void TextManager::addRoom(RoomId roomId)
{
	roomIdToIndexMap_[roomId] = textList_.size();

	std::vector<boost::shared_ptr<RenderableText>> textItemsForRoom;
	std::vector<boost::shared_ptr<Position>> layerPositionsForRoom;
	
	textList_.push_back(textItemsForRoom);
	layerPositions_.push_back(layerPositionsForRoom);
}

void TextManager::addText(RoomId roomId, boost::shared_ptr<RenderableText> renderableText)
{
	int roomIndex = roomIdToIndexMap_[roomId];

	int roomCount = textList_.size();

	if (roomIndex < roomCount)
	{
		renderableText->debugger_ = debugger_;
		renderableText->fontManager_ = fontManager_;
		
		renderableText->roomIndex_ = roomIndex;

		renderableText->previousX_ = renderableText->x_;
		renderableText->previousY_ = renderableText->y_;
		
		renderableText->renderableTextDestroySignal_->connect(boost::bind(&TextManager::renderableTextDestroyed, this, _1, _2));

		renderableText->preInitialize();
		
		textList_[roomIndex].push_back(renderableText);
	}
}

void TextManager::addTextPy(RoomId roomId, PyObj pyRenderableText)
{
	PythonReleaseGil unlocker;

	int roomIndex = roomIdToIndexMap_[roomId];

	int roomCount = textList_.size();

	if (roomIndex < roomCount)
	{
		try
		{
			PythonAcquireGil lock;

			std::string object_classname = boost::python::extract<std::string>(pyRenderableText.attr("__class__").attr("__name__"));

			boost::shared_ptr<RenderableText> renderableText = boost::shared_ptr<RenderableText>(nullptr); // (new RenderableText());

			if (object_classname == "BasicRenderableText")
			{
				boost::shared_ptr<BasicRenderableText>& basicRenderableText = extract<boost::shared_ptr<BasicRenderableText>&>(pyRenderableText);

				renderableText = boost::static_pointer_cast<RenderableText>(basicRenderableText);
			}
			else
			{
				renderableText = extract<boost::shared_ptr<RenderableText>&>(pyRenderableText);
			}

			if (renderableText == nullptr)
			{
				std::cout << "Failed to add renderable text. Renderable text object is null." << std::endl;
				
				return;
			}

			// Reset isRemoved, in case a renderable text is being added again.
			renderableText->isRemoved_ = false;

			renderableText->roomIndex_ = roomIndex;

			renderableText->previousX_ = renderableText->x_;
			renderableText->previousY_ = renderableText->y_;

			renderableText->isPyObject_ = true;
			
			renderableText->debugger_ = debugger_;
			renderableText->fontManager_ = fontManager_;

			renderableText->renderableTextDestroySignal_->connect(boost::bind(&TextManager::renderableTextDestroyed, this, _1, _2));

			if (PyObject_HasAttrString(pyRenderableText.ptr(), "added") == true)
			{
				renderableText->pyRender_ = pyRenderableText.attr("added");
				renderableText->implementsPyAdded_ = true;
			}

			if (PyObject_HasAttrString(pyRenderableText.ptr(), "buttonDown") == true)
			{
				renderableText->pyButtonDown_ = pyRenderableText.attr("buttonDown");
				renderableText->implementsPyButtonDown_ = true;
			}

			if (PyObject_HasAttrString(pyRenderableText.ptr(), "buttonUp") == true)
			{
				renderableText->pyButtonUp_ = pyRenderableText.attr("buttonUp");
				renderableText->implementsPyButtonUp_ = true;
			}

			if (PyObject_HasAttrString(pyRenderableText.ptr(), "initialize") == true)
			{
				renderableText->pyInitialize_ = pyRenderableText.attr("initialize");
				renderableText->implementsPyInitialize_ = true;
			}

			if (PyObject_HasAttrString(pyRenderableText.ptr(), "update") == true)
			{
				renderableText->pyUpdate_ = pyRenderableText.attr("update");
				renderableText->implementsPyUpdate_ = true;
			}

			if (PyObject_HasAttrString(pyRenderableText.ptr(), "removed") == true)
			{
				renderableText->pyRemoved_ = pyRenderableText.attr("removed");
				renderableText->implementsPyRemoved_ = true;
			}

			if (PyObject_HasAttrString(pyRenderableText.ptr(), "render") == true)
			{
				renderableText->pyRender_ = pyRenderableText.attr("render");
				renderableText->implementsPyRender_ = true;
			}

			pyRenderableText.attr("_color") = renderableText->color_;
			pyRenderableText.attr("_displayText") = renderableText->displayText_;
			pyRenderableText.attr("_fontName") = renderableText->fontName_;
			pyRenderableText.attr("fontManager") = ptr(renderableText->fontManager_.get());
			pyRenderableText.attr("controller") = ptr(renderableText->controller_.get());
			pyRenderableText.attr("renderer") = ptr(renderer_.get());

			renderableText->preInitialize();

			textList_[roomIndex].push_back(renderableText);
		}
		catch (boost::python::error_already_set  &)
		{
			debugger_->handlePythonError();
		}
	}
}

void TextManager::buttonDown(boost::shared_ptr<InputEvent> inputEvent)
{
	InputChannel inputChannel = inputEvent->getChannel();

	if (inputChannel_ == inputChannel)
	{
		GameButtonId buttonId = inputEvent->getButtonId();

		RoomId roomId = showingRoomId_;
		int roomIndex = roomIdToIndexMap_[roomId];
		
		int size = textList_[roomIndex].size();

		for (int i = 0; i < size; i++)
		{
			textList_[roomIndex][i]->preButtonDown(buttonId);
		}
	}
}

void TextManager::buttonUp(boost::shared_ptr<InputEvent> inputEvent)
{
	InputChannel inputChannel = inputEvent->getChannel();

	if (inputChannel_ == inputChannel)
	{
		GameButtonId buttonId = inputEvent->getButtonId();

		RoomId roomId = showingRoomId_;
		int roomIndex = roomIdToIndexMap_[roomId];

		int size = textList_[roomIndex].size();

		for (int i = 0; i < size; i++)
		{
			textList_[roomIndex][i]->preButtonUp(buttonId);
		}
	}
}

void TextManager::changeInputChannel(InputChannel oldInputChannel, InputChannel newInputChannel)
{
	if (inputChannel_ == oldInputChannel)
	{
		inputChannel_ = newInputChannel;
	}
}

void TextManager::renderableTextDestroyed(int roomIndex, int id)
{
	int roomCount = layerPositions_.size();

	if (roomIndex < roomCount)
	{
		int size = textList_[roomIndex].size();

		for (int i = 0; i < size; i++)
		{
			if (textList_[roomIndex][i]->id_ == id)
			{
				textList_[roomIndex][i]->cleanup();
				textList_[roomIndex].erase(textList_[roomIndex].begin() + i);
				break;
			}
		}

	}
}

void TextManager::addLayerPosition(int roomIndex, boost::shared_ptr<Position> pos)
{
	int roomCount = layerPositions_.size();

	if (roomIndex < roomCount)
	{
		layerPositions_[roomIndex].push_back(pos);
	}
}
//
//void TextManager::resetTextPosition(std::string textName)
//{
//	//// Render all the current text
//	//int size = textList_.size();
//	//for (int i = 0; i < size; i++)
//	//{
//	//	// Get the bitmapfont entity associated with this text.
//	//	if (textList_[i]->textName_ == textName)
//	//	{
//	//		textList_[i]->previousX_ = textList_[i]->x_;
//	//		textList_[i]->previousY_ = textList_[i]->y_;
//	//		break;
//	//	}
//	//}
//}