#include "..\..\Headers\EngineCore\Ui.hpp"

using namespace firemelon;

using namespace boost::assign;
using namespace boost::property_tree;
using namespace boost::python;

Ui::Ui()
{
	inputChannel_ = 0;
}

Ui::~Ui()
{
}

void Ui::loadPanels()
{
	// Load the panel data from the JSON file.
	try
	{
		UiPanelPtr rootPanel = uiPanelContainer_->getRoot();

		// Create a root
		ptree root;

		// Load the json file in this ptree
		read_json("panels.json", root);

		loadChildPanelsFromNode(root.get_child("elements"), rootPanel);
	}
	catch (json_parser::json_parser_error &je)
	{
		std::cout << "Error parsing UI panel data from " << je.filename() << " on line: " << je.line() << std::endl;
		std::cout << je.message() << std::endl;
	}
}

void Ui::loadChildPanelsFromNode(ptree rootPanelsNode, UiPanelPtr parent)
{
	try
	{
		// Populate mappings from string to enum values.
		std::map<std::string, PanelPositionStyle> panelPositionMap = map_list_of("MANUAL", PANEL_POSITION_MANUAL)
			("AUTO", PANEL_POSITION_AUTO);

		std::map<std::string, PanelHorizontalAlignment> panelHorizontalAlignmentMap = map_list_of("LEFT", PANEL_HORIZONTAL_ALIGNMENT_LEFT)
			("RIGHT", PANEL_HORIZONTAL_ALIGNMENT_RIGHT)
			("CENTER", PANEL_HORIZONTAL_ALIGNMENT_CENTER);

		std::map<std::string, PanelVerticalAlignment> panelVerticalAlignmentMap = map_list_of("TOP", PANEL_VERTICAL_ALIGNMENT_TOP)
			("BOTTOM", PANEL_VERTICAL_ALIGNMENT_BOTTOM)
			("CENTER", PANEL_VERTICAL_ALIGNMENT_CENTER);

		std::map<std::string, PanelElementType> panelElementTypeMap = map_list_of("PANEL", PANEL_ELEMENT_PANEL)
			("WIDGET", PANEL_ELEMENT_WIDGET);

		std::map<std::string, PanelControlFlow> panelControlFlowMap = map_list_of("BOOK", PANEL_CONTROL_FLOW_BOOK)
			("COLLAGE", PANEL_CONTROL_FLOW_COLLAGE);

		// Loop through the root panels list.
		BOOST_FOREACH(ptree::value_type const& v, rootPanelsNode)
		{
			boost::shared_ptr<UiPanelElementDefinition> itemDef = boost::make_shared<UiPanelElementDefinition>(UiPanelElementDefinition());

			const boost::property_tree::ptree & childNode = v.second;
			
			itemDef->name_ = childNode.get<std::string>("name");

			// Determine if this is a widget or a panel child.
			std::string elementTypeName = childNode.get<std::string>("elementType");

			boost::to_upper(elementTypeName);

			itemDef->panelElementType_ = panelElementTypeMap[elementTypeName];

			std::string isVisibleString = childNode.get<std::string>("visible");

			boost::to_upper(isVisibleString);

			if (isVisibleString == "TRUE")
			{
				itemDef->visible_ = true;
			}

			ptree paddingNode = childNode.get_child("padding");

			itemDef->paddingLeft_ = paddingNode.get<int>("left");

			itemDef->paddingTop_ = paddingNode.get<int>("top");

			itemDef->paddingRight_ = paddingNode.get<int>("right");

			itemDef->paddingBottom_ = paddingNode.get<int>("bottom");

			std::string focusableString = childNode.get<std::string>("focusable");

			boost::to_upper(focusableString);

			if (focusableString == "TRUE")
			{
				itemDef->focusable_ = true;
			}

			itemDef->backgroundSheetName_ = childNode.get<std::string>("background");

			switch (itemDef->panelElementType_)
			{
			case PANEL_ELEMENT_WIDGET:
			{
				// The widget node has three child nodes. The name, the type, and the list of parameters.
				std::string type = childNode.get<std::string>("type");

				itemDef->type_ = type;

				boost::to_upper(type);

				std::string widgetTypeIdName = "UIWIDGET_" + type;

				itemDef->uiWidgetId_ = ids_->getIdFromName(widgetTypeIdName);

				itemDef->buttonDownHandler_ = childNode.get<std::string>("buttonDownHandler");

				itemDef->buttonUpHandler_ = childNode.get<std::string>("buttonUpHandler");

				itemDef->selectElementHandler_ = childNode.get<std::string>("selectElementHandler");

				itemDef->gotFocusHandler_ = childNode.get<std::string>("gotFocusHandler");

				itemDef->lostFocusHandler_ = childNode.get<std::string>("lostFocusHandler");

				itemDef->params_ = childNode.get<std::string>("params");

				boost::shared_ptr<UiPanelElement> newPanelElement = constructUiPanelElement(itemDef);
				
				UiWidgetPtr newWidget = boost::static_pointer_cast<UiWidget>(newPanelElement);

				newPanelElement->indexInParent_ = parent->childElements_.size();

				parent->childElements_.push_back(newPanelElement);

				// Need both because the UI Element can't actually access the panel, otherwise it runs into a circular dependency issue.
				newWidget->parentPanel_ = parent;

				newWidget->parentElement_ = parent;

				uiWidgetContainer_->add(newWidget);

				break;
			}
			case PANEL_ELEMENT_PANEL:
			{
				// Read the panel properties.
				itemDef->layoutName_ = childNode.get<std::string>("layout");
				
				ptree positionNode = childNode.get_child("position");

				itemDef->positionX_ = positionNode.get<int>("x");

				itemDef->positionY_ = positionNode.get<int>("y");

				// Read the caption properties.
				ptree captionNode = childNode.get_child("caption");

				itemDef->caption_ = captionNode.get<std::string>("text");

				itemDef->captionFont_ = captionNode.get<std::string>("font");

				ptree captionPositionNode = captionNode.get_child("position");

				itemDef->captionPositionLeft_ = captionPositionNode.get<float>("left");

				itemDef->captionPositionTop_ = captionPositionNode.get<float>("top");

				ptree captionColorNode = captionNode.get_child("color");

				itemDef->captionColorRed_ = captionColorNode.get<float>("red");

				itemDef->captionColorGreen_ = captionColorNode.get<float>("green");

				itemDef->captionColorBlue_ = captionColorNode.get<float>("blue");

				itemDef->captionScale_ = captionNode.get<float>("scale");

				itemDef->shownHandler_ = childNode.get<std::string>("shownHandler");

				itemDef->hiddenHandler_ = childNode.get<std::string>("hiddenHandler");

				// Read the alignment properties.
				std::string horizontalAlignmentName = childNode.get<std::string>("horizontalAlignment");

				boost::to_upper(horizontalAlignmentName);

				itemDef->horizontalAlignment_ = panelHorizontalAlignmentMap[horizontalAlignmentName];

				std::string verticalAlignmentName = childNode.get<std::string>("verticalAlignment");

				boost::to_upper(verticalAlignmentName);

				itemDef->verticalAlignment_ = panelVerticalAlignmentMap[verticalAlignmentName];

				std::string positionStyleName = childNode.get<std::string>("positionStyle");

				boost::to_upper(positionStyleName);

				itemDef->positionStyle_ = panelPositionMap[positionStyleName];

				std::string focusWrapString = childNode.get<std::string>("focusWrap");

				boost::to_upper(focusWrapString);

				bool focusWrap = false;

				if (focusWrapString == "TRUE")
				{
					itemDef->focusWrap_ = true;
				}

				std::string controlFlowName = childNode.get<std::string>("controlFlow");

				boost::to_upper(controlFlowName);

				itemDef->controlFlow_ = panelControlFlowMap[controlFlowName];

				std::string fillBottomString = childNode.get<std::string>("fillBottom");

				boost::to_upper(fillBottomString);

				if (fillBottomString == "TRUE")
				{
					itemDef->fillBottom_ = true;
				}

				std::string fillLeftString = childNode.get<std::string>("fillLeft");

				boost::to_upper(fillLeftString);

				if (fillLeftString == "TRUE")
				{
					itemDef->fillLeft_ = true;
				}

				std::string fillRightString = childNode.get<std::string>("fillRight");

				boost::to_upper(fillRightString);

				if (fillRightString == "TRUE")
				{
					itemDef->fillRight_ = true;
				}

				std::string fillTopString = childNode.get<std::string>("fillTop");

				boost::to_upper(fillTopString);

				if (fillTopString == "TRUE")
				{
					itemDef->fillTop_ = true;
				}

				ptree marginsNode = childNode.get_child("margins");

				itemDef->marginLeft_ = marginsNode.get<int>("left");

				itemDef->marginTop_ = marginsNode.get<int>("top");

				itemDef->marginRight_ = marginsNode.get<int>("right");

				itemDef->marginBottom_ = marginsNode.get<int>("bottom");

				ptree frameMarginsNode = childNode.get_child("frameMargins");

				itemDef->frameMarginLeft_ = frameMarginsNode.get<int>("left");

				itemDef->frameMarginTop_ = frameMarginsNode.get<int>("top");

				itemDef->frameMarginRight_ = frameMarginsNode.get<int>("right");

				itemDef->frameMarginBottom_ = frameMarginsNode.get<int>("bottom");

				ptree borderNode = childNode.get_child("border");

				itemDef->borderLeft_ = borderNode.get<int>("left");

				itemDef->borderTop_ = borderNode.get<int>("top");

				itemDef->borderRight_ = borderNode.get<int>("right");

				itemDef->borderBottom_ = borderNode.get<int>("bottom");
				
				// Loop through the border decoration list.
				ptree borderDecorationsListNode = childNode.get_child("borderDecorations");

				BOOST_FOREACH(ptree::value_type const& v, borderDecorationsListNode)
				{
					const boost::property_tree::ptree & decorationNode = v.second;

					std::string decorationSheetName = decorationNode.get<std::string>("sheet");

					int row = decorationNode.get<int>("row");

					int column = decorationNode.get<int>("column");

					ptree positionNode = decorationNode.get_child("position");

					float left = positionNode.get<float>("left");

					float top = positionNode.get<float>("top");

					std::string origin = decorationNode.get<std::string>("origin");

					PanelDecoration decoration;

					decoration.sheetName = decorationSheetName;
					decoration.row = row;
					decoration.column = column;
					decoration.left = left;
					decoration.top = top;

					if (origin == "TopMiddle")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_TOPMIDDLE;
					}
					else if (origin == "TopRight")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_TOPRIGHT;
					}
					else if (origin == "CenterLeft")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_CENTERLEFT;
					}
					else if (origin == "Center")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_CENTER;
					}
					else if (origin == "CenterRight")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_CENTERRIGHT;
					}
					else if (origin == "BottomLeft")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_BOTTOMLEFT;
					}
					else if (origin == "BottomMiddle")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_BOTTOMMIDDLE;
					}
					else if (origin == "BottomRight")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_BOTTOMRIGHT;
					}
					else
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_TOPLEFT;
					}


					itemDef->borderDecorations_.push_back(decoration);
				}

				// Loop through the decoration list.
				ptree decorationsListNode = childNode.get_child("decorations");

				BOOST_FOREACH(ptree::value_type const& v, decorationsListNode)
				{
					const boost::property_tree::ptree & decorationNode = v.second;

					std::string decorationSheetName = decorationNode.get<std::string>("sheet");

					int row = decorationNode.get<int>("row");

					int column = decorationNode.get<int>("column");

					ptree positionNode = decorationNode.get_child("position");

					float left = positionNode.get<float>("left");

					float top = positionNode.get<float>("top");

					std::string origin = decorationNode.get<std::string>("origin");

					PanelDecoration decoration;

					decoration.sheetName = decorationSheetName;
					decoration.row = row;
					decoration.column = column;
					decoration.left = left;
					decoration.top = top;

					if (origin == "TopMiddle")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_TOPMIDDLE;
					}
					else if (origin == "TopRight")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_TOPRIGHT;
					}
					else if (origin == "CenterLeft")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_CENTERLEFT;
					}
					else if (origin == "Center")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_CENTER;
					}
					else if (origin == "CenterRight")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_CENTERRIGHT;
					}
					else if (origin == "BottomLeft")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_BOTTOMLEFT;
					}
					else if (origin == "BottomMiddle")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_BOTTOMMIDDLE;
					}
					else if (origin == "BottomRight")
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_BOTTOMRIGHT;
					}
					else
					{
						decoration.origin = PANEL_DECORATION_ORIGIN_TOPLEFT;
					}


					itemDef->decorations_.push_back(decoration);
				}

				boost::shared_ptr<UiPanelElement> newPanelElement = constructUiPanelElement(itemDef);

				UiPanelPtr newPanel = boost::static_pointer_cast<UiPanel>(newPanelElement);

				uiPanelContainer_->add(newPanel);


				newPanelElement->indexInParent_ = parent->childElements_.size();

				parent->childElements_.push_back(newPanelElement);

				switch (parent->controlFlow_)
				{
				case PANEL_CONTROL_FLOW_BOOK:

					// When using book control, default the first child's visible property to true.
					if (parent->childPanels_.size() == 0)
					{
						newPanel->setIsVisible(true);
					}
					else
					{
						newPanel->setIsVisible(false);
					}

					break;
				}

				parent->childPanels_.push_back(newPanel);

				// Need both because the UI Element can't actually access the panel, otherwise it runs into a circular dependency issue.
				newPanel->parentPanel_ = parent;

				newPanel->parentElement_ = parent;

				// Load the child panels.
				ptree childElementsNode = childNode.get_child("elements");

				loadChildPanelsFromNode(childElementsNode, newPanel);

				break;
			}
			}
		}
	}	
	catch (const ptree_error &e)
	{
		std::cout << "Error parsing UI panel data: " << e.what() << std::endl;
	}
}

void Ui::render()
{	
	// Turn off the fade effect when rendering the UI.
	renderer_->setEnableFade(false);

	/* 
	The way rendering works is that every element has 5 rectangles. The background,
	the border, the padding, the margins, and the content.

	  - The background is the largest. The renderBackground function will fill this rect with
	    whatever is set as the background sheet 9-box.

	  - The border rect defines the size of the border in the 9-box. It will be slightly smaller
	    than the background. Its purpose is so that the padding, margins, or content rects do
		not get placed underneath the border.
	  
	  - The padding, margins, and content rect make up the "main" region of a panel. The size of 
	    the main region is automatically sizes to be the minimum bounding size of all of the child 
		elements main regions.

	  - The padding rect defines an area outside of the main region which will provide spacing
	    between any other elements which share the same parent. The dotted outline in the 
		crude diagram below represents the padding rect around one child element. Without any
		padding, the two would be contiguous to each other.

		 ______________________________________________
		|                                              |
		|         .............                        |
		|         :   _____   : _____                  |
		|         :  |     |  :|     |                 |
		|         :  |     |  :|     |                 |
		|         :  |_____|  :|_____|                 |
		|         :...........:                        |
		|                                              |
		|                                              |
		|                                              |
		|______________________________________________|

	  - The margin rect is somewhat similar to the padding rect, except instead of adding spacing
	    between two elements who share a parent, it adds spacing around the sides of an elements main
		region. The crude diagram below illustrates this. The dotted line is the margins of the outer
		edges of the main region, and inside the margins are two stacked elements.
		 ______________________________________________
		|                                              |
		|              ________________                |
		|             |                |               |
		|             |  ............  |               |
		|             |  :|````````|:  |               |
		|             |  :|________|:  |               |
		|             |  :|________|:  |               |
		|             |  :..........:  |               |
		|             |________________|               |
		|                                              |
		|______________________________________________|

	  - The content rect is the region where the actual child elements of the panel will be rendered.

    All of the panels are stored in a tree structure. To do the rendering in the correct order, a pre-order
	traversal is done. This allows background (parent) elements to be rendered before their child elements.

	Each panel uses its own coordinate space, using the parent content rect's top lefthand corner as
	its origin point.
	
	*/

	uiPanelContainer_->getRoot()->renderBase(0, 0); 
	
	// Now turn it back on again.
	renderer_->setEnableFade(true);
}

void Ui::initializeBase()
{
	inputDeviceManager_->buttonDownSignal_.connect(boost::bind(&Ui::preButtonDown, this, _1));
	inputDeviceManager_->buttonUpSignal_.connect(boost::bind(&Ui::preButtonUp, this, _1));
	inputDeviceManager_->changeChannelOfListenersSignal_.connect(boost::bind(&Ui::changeInputChannel, this, _1, _2));
	
	initializePythonData();

	initialize();

	initialized();
}

void Ui::initializePythonData()
{	
	PythonAcquireGil lock;

	try
	{
		// The scripts should be loaded just once and error if they are missing.
		std::string sCode = "from ";

		std::string scriptVar = "ui";

		sCode += scriptName_ + " import " + scriptTypeName_ + "\n";
		sCode += scriptVar + " = " + scriptTypeName_ + "()";
	
		pyMainModule_ = import("__main__");
		pyMainNamespace_ = pyMainModule_.attr("__dict__");
	
		str pyCode(sCode);	
		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);

		// Get the instance for this object.
		pyUiInstance_ = extract<object>(pyMainNamespace_[scriptVar]);
		pyUiNamespace_ = pyUiInstance_.attr("__dict__");
	
		// Import firemelon module to the instance.
		object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
		pyUiNamespace_["firemelon"] = pyFiremelonModule;

		// Store the functions as python objects.
		pyButtonDown_ = pyUiInstance_.attr("buttonDown");
		pyButtonUp_ = pyUiInstance_.attr("buttonUp");
		pyInitialized_ = pyUiInstance_.attr("initialized");

	}
	catch(boost::python::error_already_set  &)
	{
		std::cout<<"Error loading UI " + scriptTypeName_<<std::endl;
		debugger_->handlePythonError();
	}
}

void Ui::initialize()
{
}

void Ui::initialized()
{
	PythonAcquireGil lock;

	try
	{
		pyInitialized_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Ui::update(double time)
{
	for (size_t i = 0; i < queuedCommands_.size(); i++)
	{
		UiCommand command = queuedCommands_[i];

		boost::shared_ptr<UiFocusArgs> focusArgs;
		boost::shared_ptr<UiCommandArgs> args;

		switch (command.type)
		{
		case UI_COMMAND_FOCUS:

			focusArgs = boost::static_pointer_cast<UiFocusArgs>(command.args);

			switch (focusArgs->focusType)
			{
			case FOCUS_BY_NAME:

				focusElementInternal(focusArgs->panelName, focusArgs->elementName);

				focusArgs->focusType = FOCUS_NONE;

				break;

			case FOCUS_FIRST:

				focusFirstElementInternal(focusArgs->panelName);

				focusArgs->focusType = FOCUS_NONE;

				break;

			case FOCUS_LAST:

				focusLastElementInternal(focusArgs->panelName);

				focusArgs->focusType = FOCUS_NONE;

				break;

			case FOCUS_NEXT:

				focusNextElementInternal(focusArgs->panelName);

				focusArgs->focusType = FOCUS_NONE;

				break;

			case FOCUS_PREVIOUS:

				focusPreviousElementInternal(focusArgs->panelName);

				focusArgs->focusType = FOCUS_NONE;

				break;
			}

			break;
		
		case UI_COMMAND_SELECT:

			selectElementInternal();

			break;

		case UI_COMMAND_SHOWPANEL:
			
			showPanelInternal(command.args->elementName);

			break;

		case UI_COMMAND_HIDEPANEL:

			hidePanelInternal(command.args->elementName);

			break;
		}
	}

	queuedCommands_.clear();

	UiPanelPtr root = uiPanelContainer_->getRoot();

	// Update all of the panel elementss using a recursive post-order tree traveral.
	// This is necessary because panel size is determined by the size of its children,
	// so all of the child sizes must be calculated before the parent.
	root->calculateMinimumBoundingSizeBase();

	// Now that the minimum sizes and inflated sizes are known, position the elements
	// within the panels.
	root->locateElementsBase();

	root->updateBase(time);
}

boost::shared_ptr<Size> Ui::getPanelSize(std::string panelName)
{
	auto panel = getPanelByName(panelName);

	return panel->getFullSize();
}

boost::shared_ptr<Size> Ui::getPanelSizePy(std::string panelName)
{
	PythonReleaseGil unlocker;

	return getPanelSize(panelName);
}

void Ui::cleanup()
{

}

void Ui::cleanupPythonData()
{
	PythonAcquireGil lock;
	
	try
	{
		std::string sCode = "ui = None";
		str pyCode(sCode);
	
		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);
	
		// Store the functions as python objects.
		pyMainModule_ = boost::python::object();
		pyMainNamespace_ = boost::python::object();
		pyUiInstance_ = boost::python::object();
		pyUiNamespace_ = boost::python::object();
		pyButtonDown_ = boost::python::object();
		pyButtonUp_ = boost::python::object();
		pyInitialized_ = boost::python::object();
	}
	catch (boost::python::error_already_set  &)
	{
		std::cout << "Error destroying UI manager " + scriptTypeName_ << std::endl;
		debugger_->handlePythonError();
	}
}

void Ui::cleanupBase()
{
	inputDeviceManager_->buttonDownSignal_.disconnect(boost::bind(&Ui::preButtonDown, this, _1));
	inputDeviceManager_->buttonUpSignal_.disconnect(boost::bind(&Ui::preButtonUp, this, _1));
	inputDeviceManager_->changeChannelOfListenersSignal_.disconnect(boost::bind(&Ui::changeInputChannel, this, _1, _2));

	size_t size = uiPanelContainer_->size();

	for (size_t i = 0; i < size; i++)
	{
		auto panel = uiPanelContainer_->getPanelByIndex(i);

		panel->visibilityChangedSignal_->disconnect(boost::bind(&Ui::panelVisibilityChanged, this, _1));
	}

	size = uiWidgetContainer_->size();

	for (size_t i = 0; i < size; i++)
	{
		auto widget = uiWidgetContainer_->getWidgetByIndex(i);

		widget->visibilityChangedSignal_->disconnect(boost::bind(&Ui::widgetVisibilityChanged, this, _1));
	}

	uiPanelContainer_->cleanup();

	uiWidgetContainer_->cleanup();

	cleanupPythonData();

	cleanup();
}

UiWidgetPtr Ui::getFocusedWidgetPy()
{
	PythonReleaseGil unlocker;

	return getFocusedWidget();
}

UiWidgetPtr Ui::getFocusedWidget()
{
	boost::shared_ptr<UiPanelElement> focusedElement = uiPanelContainer_->getRoot()->getFocusedWidget();

	UiWidgetPtr focusedWidget = boost::static_pointer_cast<UiWidget>(focusedElement);

	return focusedWidget;
}

UiPanelPtr Ui::getPanelByName(std::string panelName)
{
	return uiPanelContainer_->getPanelByName(panelName);
}

UiPanelPtr Ui::getPanelByNamePy(std::string panelName)
{
	PythonReleaseGil unlocker;

	return getPanelByName(panelName);
}

UiWidgetPtr Ui::getWidgetByName(std::string widgetName)
{
	return uiWidgetContainer_->getWidgetByName(widgetName);
}

UiWidgetPtr Ui::getWidgetByNamePy(std::string widgetName)
{
	PythonReleaseGil unlocker;
	
	return getWidgetByName(widgetName);
}

AudioPlayerPtr Ui::getAudioPlayer()
{
	return audioPlayer_;
}

InputDeviceManagerPtr Ui::getInputDeviceManager()
{
	return inputDeviceManager_;
}

object Ui::getPyInstance()
{
	return pyUiInstance_;
}

RendererPtr Ui::getRenderer()
{
	return renderer_;
}

bool Ui::getIsShowing()
{
	return uiPanelContainer_->getRoot()->hasVisibleChildren();
}

void Ui::preButtonDown(boost::shared_ptr<InputEvent> inputEvent)
{
	//if (inputEvent->ignoreUiInput_ == true)
	//{
	//	return;
	//}

	if (inputDeviceManager_->disableUi_ == false)
	{
		InputChannel inputChannel = inputEvent->getChannel();

		GameButtonId buttonId = inputEvent->getButtonId();

		if (inputChannel == inputChannel_)
		{
			UiWidgetPtr focusedWidget = getFocusedWidget();

			if (focusedWidget != nullptr)
			{
				if (focusedWidget->getIsVisible() == true)
				{
					focusedWidget->buttonDown(buttonId);

					widgetButtonDown(focusedWidget, buttonId);
				}
			}

			buttonDown(buttonId);
		}
	}
}

void Ui::buttonDown(GameButtonId buttonCode)
{
	PythonAcquireGil lock;

	try
	{
		pyButtonDown_(buttonCode);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Ui::widgetButtonDown(UiWidgetPtr widget, GameButtonId buttonId)
{
	PythonAcquireGil lock;

	try
	{
		if (PyObject_HasAttrString(pyUiInstance_.ptr(), widget->buttonDownHandler_.c_str()) == true)
		{
			pyUiInstance_.attr(widget->buttonDownHandler_.c_str())(widget, buttonId);
		}
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Ui::widgetButtonUp(UiWidgetPtr widget, GameButtonId buttonId)
{
	PythonAcquireGil lock;

	try
	{
		if (PyObject_HasAttrString(pyUiInstance_.ptr(), widget->buttonUpHandler_.c_str()) == true)
		{
			pyUiInstance_.attr(widget->buttonUpHandler_.c_str())(widget, buttonId);
		}
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Ui::widgetGotFocus(UiWidgetPtr widget)
{
	PythonAcquireGil lock;

	try
	{
		if (PyObject_HasAttrString(pyUiInstance_.ptr(), widget->gotFocusHandler_.c_str()) == true)
		{
			pyUiInstance_.attr(widget->gotFocusHandler_.c_str())(widget);
		}
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Ui::widgetLostFocus(UiWidgetPtr widget)
{
	PythonAcquireGil lock;

	try
	{
		if (PyObject_HasAttrString(pyUiInstance_.ptr(), widget->lostFocusHandler_.c_str()) == true)
		{
			pyUiInstance_.attr(widget->lostFocusHandler_.c_str())(widget);
		}
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}


void Ui::elementShown(UiPanelElementPtr element)
{
	PythonAcquireGil lock;

	try
	{
		if (element->elementType_ == PANEL_ELEMENT_PANEL)
		{
			UiPanelPtr panel = boost::static_pointer_cast<UiPanel>(element);

			if (PyObject_HasAttrString(pyUiInstance_.ptr(), element->shownHandler_.c_str()) == true && panel->shownEventCalled_ == false)
			{
				pyUiInstance_.attr(element->shownHandler_.c_str())(panel);

				// Only call once per frame.
				panel->shownEventCalled_ = true;
			}

			// All of the visible child elements will also be shown.
			for (size_t i = 0; i < panel->childElements_.size(); i++)
			{
				auto childElement = panel->childElements_[i];

				if (childElement->getIsVisible() == true)
				{
					elementShown(childElement);
				}						
			}
		}
		else if (element->elementType_ == PANEL_ELEMENT_WIDGET)
		{
			if (PyObject_HasAttrString(pyUiInstance_.ptr(), element->shownHandler_.c_str()) == true)
			{
				UiWidgetPtr widget = boost::static_pointer_cast<UiWidget>(element);

				pyUiInstance_.attr(element->shownHandler_.c_str())(widget);
			}
		}
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Ui::elementHidden(UiPanelElementPtr element)
{
	PythonAcquireGil lock;

	try
	{
		if (element->elementType_ == PANEL_ELEMENT_PANEL)
		{
			UiPanelPtr panel = boost::static_pointer_cast<UiPanel>(element);

			if (PyObject_HasAttrString(pyUiInstance_.ptr(), element->hiddenHandler_.c_str()) == true && panel->hiddenEventCalled_ == false)
			{
				pyUiInstance_.attr(element->hiddenHandler_.c_str())(panel);

				// Only call once per frame.
				panel->hiddenEventCalled_ = true;
			}

			// All of the visible child elements will also be hidden.
			for (size_t i = 0; i < panel->childElements_.size(); i++)
			{
				auto childElement = panel->childElements_[i];

				// Don't use getIsVisible, because it will always return false (because the parent was hidden).
				// Is visible however will retain its state regardless of what the parent's state is.
				if (childElement->isVisible_ == true)
				{
					elementHidden(childElement);
				}
			}
		}
		else if (element->elementType_ == PANEL_ELEMENT_WIDGET)
		{
			if (PyObject_HasAttrString(pyUiInstance_.ptr(), element->hiddenHandler_.c_str()) == true)
			{
				UiWidgetPtr widget = boost::static_pointer_cast<UiWidget>(element);

				pyUiInstance_.attr(element->hiddenHandler_.c_str())(widget);
			}
		}
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Ui::preButtonUp(boost::shared_ptr<InputEvent> inputEvent)
{
	if (inputDeviceManager_->disableUi_ == false)
	{
		int inputChannel = inputEvent->getChannel();

		GameButtonId buttonId = inputEvent->getButtonId();

		if (inputChannel == inputChannel_)
		{
			UiWidgetPtr focusedWidget = getFocusedWidget();

			if (focusedWidget != nullptr)
			{
				focusedWidget->buttonUp(buttonId);

				widgetButtonUp(focusedWidget, buttonId);
			}

			buttonUp(buttonId);
		}
	}
}

void Ui::buttonUp(GameButtonId buttonCode)
{
	PythonAcquireGil lock;

	try
	{
		pyButtonUp_(buttonCode);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Ui::changeInputChannel(InputChannel oldInputChannel, InputChannel newInputChannel)
{
	if (inputChannel_ == oldInputChannel)
	{
		inputChannel_ = newInputChannel;
	}
}

void Ui::selectElement()
{
	UiCommand command;

	command.type = UI_COMMAND_SELECT;

	command.args = nullptr;

	queuedCommands_.push_back(command);
}

void Ui::selectElementInternal()
{
	PythonAcquireGil lock;

	try
	{
		UiWidgetPtr focusedWidget = getFocusedWidget();

		if (focusedWidget != nullptr)
		{
			if (focusedWidget->getIsVisible() == true)
			{
				if (PyObject_HasAttrString(pyUiInstance_.ptr(), focusedWidget->selectElementHandler_.c_str()) == true)
				{
					pyUiInstance_.attr(focusedWidget->selectElementHandler_.c_str())(focusedWidget);
				}
			}
		}
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Ui::selectElementPy()
{
	PythonReleaseGil unlocker;

	return selectElement();
}

void Ui::focusElement(std::string panelName, std::string elementName)
{
	boost::shared_ptr<UiFocusArgs> focusArgs = boost::make_shared<UiFocusArgs>(UiFocusArgs());
	
	focusArgs->focusType = FOCUS_BY_NAME;

	focusArgs->panelName = panelName;

	focusArgs->elementName = elementName;

	UiCommand command;

	command.type = UI_COMMAND_FOCUS;

	command.args = focusArgs;

	queuedCommands_.push_back(command);
}

void Ui::focusElementInternal(std::string panelName, std::string elementName)
{
	auto panel = getPanelByName(panelName);

	if (panel != nullptr)
	{
		auto oldFocusedWidget = getFocusedWidget();

		if (panel->focusElement(elementName) == true)
		{
			if (oldFocusedWidget != nullptr)
			{
				widgetLostFocus(oldFocusedWidget);
			}

			auto newFocusedWidget = getFocusedWidget();

			if (newFocusedWidget != nullptr)
			{
				widgetGotFocus(newFocusedWidget);
			}
		}
	}
}

void Ui::focusElementPy(std::string panelName, std::string elementName)
{
	PythonReleaseGil unlocker;

	focusElement(panelName, elementName);
}

void Ui::focusFirstElement(std::string panelName)
{
	boost::shared_ptr<UiFocusArgs> focusArgs = boost::make_shared<UiFocusArgs>(UiFocusArgs());

	focusArgs->focusType = FOCUS_FIRST;

	focusArgs->panelName = panelName;

	UiCommand command;

	command.type = UI_COMMAND_FOCUS;

	command.args = focusArgs;

	queuedCommands_.push_back(command);
}

void Ui::focusFirstElementInternal(std::string panelName)
{
	auto panel = getPanelByName(panelName);

	if (panel != nullptr)
	{
		auto oldFocusedWidget = getFocusedWidget();

		if (panel->focusFirstElement() == true)
		{
			if (oldFocusedWidget != nullptr)
			{
				widgetLostFocus(oldFocusedWidget);
			}

			auto newFocusedWidget = getFocusedWidget();

			if (newFocusedWidget != nullptr)
			{
				widgetGotFocus(newFocusedWidget);
			}
		}
	}
}

void Ui::focusFirstElementPy(std::string panelName)
{
	PythonReleaseGil unlocker;

	focusFirstElement(panelName);
}

void Ui::focusLastElement(std::string panelName)
{
	boost::shared_ptr<UiFocusArgs> focusArgs = boost::make_shared<UiFocusArgs>(UiFocusArgs());

	focusArgs->focusType = FOCUS_LAST;

	focusArgs->panelName = panelName;
	
	UiCommand command;

	command.type = UI_COMMAND_FOCUS;

	command.args = focusArgs;

	queuedCommands_.push_back(command);
}

void Ui::focusLastElementInternal(std::string panelName)
{
	auto panel = getPanelByName(panelName);

	if (panel != nullptr)
	{
		auto oldFocusedWidget = getFocusedWidget();

		if (panel->focusLastElement() == true)
		{
			if (oldFocusedWidget != nullptr)
			{
				widgetLostFocus(oldFocusedWidget);
			}

			auto newFocusedWidget = getFocusedWidget();

			if (newFocusedWidget != nullptr)
			{
				widgetGotFocus(newFocusedWidget);
			}
		}
	}
}

void Ui::focusLastElementPy(std::string panelName)
{
	PythonReleaseGil unlocker;

	focusLastElement(panelName);
}

void Ui::focusNextElement(std::string panelName)
{
	boost::shared_ptr<UiFocusArgs> focusArgs = boost::make_shared<UiFocusArgs>(UiFocusArgs());

	focusArgs->focusType = FOCUS_NEXT;

	focusArgs->panelName = panelName;

	UiCommand command;

	command.type = UI_COMMAND_FOCUS;

	command.args = focusArgs;

	queuedCommands_.push_back(command);
}

void Ui::focusNextElementInternal(std::string panelName)
{
	auto panel = getPanelByName(panelName);

	if (panel != nullptr)
	{
		auto oldFocusedWidget = getFocusedWidget();

		if (panel->focusNextElement() == true)
		{
			if (oldFocusedWidget != nullptr)
			{
				widgetLostFocus(oldFocusedWidget);
			}

			auto newFocusedWidget = getFocusedWidget();

			if (newFocusedWidget != nullptr)
			{
				widgetGotFocus(newFocusedWidget);
			}
		}
	}
}

void Ui::focusNextElementPy(std::string panelName)
{
	PythonReleaseGil unlocker;

	focusNextElement(panelName);
}


void Ui::focusPreviousElement(std::string panelName)
{
	boost::shared_ptr<UiFocusArgs> focusArgs = boost::make_shared<UiFocusArgs>(UiFocusArgs());

	focusArgs->focusType = FOCUS_PREVIOUS;

	focusArgs->panelName = panelName;

	UiCommand command;

	command.type = UI_COMMAND_FOCUS;

	command.args = focusArgs;

	queuedCommands_.push_back(command);
}

void Ui::focusPreviousElementInternal(std::string panelName)
{
	auto panel = getPanelByName(panelName);

	if (panel != nullptr)
	{
		auto oldFocusedWidget = getFocusedWidget();

		if (panel->focusPreviousElement() == true)
		{
			if (oldFocusedWidget != nullptr)
			{
				widgetLostFocus(oldFocusedWidget);
			}

			auto newFocusedWidget = getFocusedWidget();

			if (newFocusedWidget != nullptr)
			{
				widgetGotFocus(newFocusedWidget);
			}
		}
	}
}

void Ui::focusPreviousElementPy(std::string panelName)
{
	PythonReleaseGil unlocker;

	focusPreviousElement(panelName);
}

void Ui::createUiPanelElementPy(boost::shared_ptr<UiPanel> parent, boost::shared_ptr<UiPanelElementDefinition> itemDef)
{
	PythonReleaseGil unlocker;

	createUiPanelElement(parent, itemDef);
}

void Ui::createUiPanelElement(boost::shared_ptr<UiPanel> parent, boost::shared_ptr<UiPanelElementDefinition> itemDef)
{
	if (itemDef == nullptr)
	{
		std::cout << "Null panel element definition provided. Skipping element creation." << std::endl;

		return;
	}
	else if (parent == nullptr)
	{
		std::cout << "Null parent element provided. Skipping element creation." << std::endl;

		return;
	}

	boost::shared_ptr<UiPanelElement> newPanelElement = constructUiPanelElement(itemDef);
	
	if (newPanelElement == nullptr)
	{
		// Invalid parameters used when creating element.
		return;
	}

	switch (itemDef->panelElementType_)
	{
	case PANEL_ELEMENT_WIDGET:
	{
		UiWidgetPtr newWidget = boost::static_pointer_cast<UiWidget>(newPanelElement);

		newPanelElement->indexInParent_ = parent->childElements_.size();

		newPanelElement->visibilityChangedSignal_->connect(boost::bind(&Ui::widgetVisibilityChanged, this, _1));

		parent->childElements_.push_back(newPanelElement);

		// Need both because the UI Element can't actually access the panel, otherwise it runs into a circular dependency issue.
		newWidget->parentPanel_ = parent;

		newWidget->parentElement_ = parent;

		uiWidgetContainer_->add(newWidget);

		break;
	}
	case PANEL_ELEMENT_PANEL:
	{
		UiPanelPtr newPanel = boost::static_pointer_cast<UiPanel>(newPanelElement);

		uiPanelContainer_->add(newPanel);

		newPanelElement->indexInParent_ = parent->childElements_.size();

		newPanelElement->visibilityChangedSignal_->connect(boost::bind(&Ui::panelVisibilityChanged, this, _1));

		parent->childElements_.push_back(newPanelElement);

		switch (parent->controlFlow_)
		{
		case PANEL_CONTROL_FLOW_BOOK:

			// When using book control, default the first child's visible property to true.
			if (parent->childPanels_.size() == 0)
			{
				newPanel->setIsVisible(true);
			}
			else
			{
				newPanel->setIsVisible(false);
			}

			break;
		}

		parent->childPanels_.push_back(newPanel);

		// Need both because the UI Element can't actually access the panel, otherwise it runs into a circular dependency issue.
		newPanel->parentPanel_ = parent;

		newPanel->parentElement_ = parent;

		// Load the child elements.
		for (size_t i = 0; i < itemDef->childElements_.size(); i++)
		{
			createUiPanelElement(newPanel, itemDef->childElements_[i]);
		}

		// Reset the new panel to make sure a focuable element is focused on.
		newPanel->reset();

		break;
	}
	}
}


UiPanelElementPtr Ui::constructUiPanelElement(boost::shared_ptr<UiPanelElementDefinition> itemDef)
{
	boost::shared_ptr<UiPanelElement> newPanelElement;

	if (itemDef->panelElementType_ == PANEL_ELEMENT_PANEL)
	{
		newPanelElement = constructUiPanel(itemDef);

		newPanelElement->visibilityChangedSignal_->connect(boost::bind(&Ui::panelVisibilityChanged, this, _1));
	}
	else if(itemDef->panelElementType_ == PANEL_ELEMENT_WIDGET)
	{
		newPanelElement = constructUiWidget(itemDef); 
		
		newPanelElement->visibilityChangedSignal_->connect(boost::bind(&Ui::widgetVisibilityChanged, this, _1));
	}

	if (newPanelElement != nullptr)
	{
		newPanelElement->isVisible_ = itemDef->visible_;

		newPanelElement->renderer_ = renderer_;

		newPanelElement->backgroundSheetName_ = itemDef->backgroundSheetName_;

		newPanelElement->debugger_ = debugger_;

		newPanelElement->fontManager_ = fontManager_;

		newPanelElement->paddingLeft_ = itemDef->paddingLeft_;

		newPanelElement->paddingTop_ = itemDef->paddingTop_;

		newPanelElement->paddingRight_ = itemDef->paddingRight_;

		newPanelElement->paddingBottom_ = itemDef->paddingBottom_;

		newPanelElement->borderLeft_ = itemDef->borderLeft_;

		newPanelElement->borderTop_ = itemDef->borderTop_;

		newPanelElement->borderRight_ = itemDef->borderRight_;

		newPanelElement->borderBottom_ = itemDef->borderBottom_;

		newPanelElement->focusable_ = itemDef->focusable_;

		newPanelElement->hiddenHandler_ = itemDef->hiddenHandler_;

		newPanelElement->shownHandler_ = itemDef->shownHandler_;
	}

	return newPanelElement;
}

UiPanelPtr Ui::constructUiPanel(boost::shared_ptr<UiPanelElementDefinition> itemDef)
{
	UiPanelPtr newPanel = uiPanelFactory_->createUiPanelBase(itemDef->layoutName_, itemDef->name_);
	
	newPanel->caption_ = itemDef->caption_;

	newPanel->captionFont_ = itemDef->captionFont_;

	newPanel->captionLeft_ = itemDef->captionPositionLeft_;

	newPanel->captionTop_ = itemDef->captionPositionTop_;

	newPanel->captionRenderEffects_->getHueColor()->setR(itemDef->captionColorRed_);

	newPanel->captionRenderEffects_->getHueColor()->setG(itemDef->captionColorGreen_);

	newPanel->captionRenderEffects_->getHueColor()->setB(itemDef->captionColorBlue_);

	newPanel->captionRenderEffects_->setScaleFactor(itemDef->captionScale_);

	newPanel->setHorizontalAlignment(itemDef->horizontalAlignment_);

	newPanel->setVerticalAlignment(itemDef->verticalAlignment_);

	newPanel->fillBottom_ = itemDef->fillBottom_;

	newPanel->fillLeft_ = itemDef->fillLeft_;

	newPanel->fillRight_ = itemDef->fillRight_;

	newPanel->fillTop_ = itemDef->fillTop_;

	newPanel->focusWrap_ = itemDef->focusWrap_;

	newPanel->marginLeft_ = itemDef->marginLeft_;

	newPanel->marginTop_ = itemDef->marginTop_;

	newPanel->marginRight_ = itemDef->marginRight_;

	newPanel->marginBottom_ = itemDef->marginBottom_;

	newPanel->position_->setX(itemDef->positionX_);

	newPanel->position_->setY(itemDef->positionY_);

	newPanel->frameMarginLeft_ = itemDef->frameMarginLeft_;

	newPanel->frameMarginTop_ = itemDef->frameMarginTop_;

	newPanel->frameMarginRight_ = itemDef->frameMarginRight_;

	newPanel->frameMarginBottom_ = itemDef->frameMarginBottom_;

	newPanel->controlFlow_ = itemDef->controlFlow_;

	newPanel->positionStyle_ = itemDef->positionStyle_;

	newPanel->backgroundSheetName_ = itemDef->backgroundSheetName_;

	for (size_t i = 0; i < itemDef->borderDecorations_.size(); i++)
	{
		newPanel->borderDecorations_.push_back(itemDef->borderDecorations_[i]);
	}

	for (size_t i = 0; i < itemDef->decorations_.size(); i++)
	{
		newPanel->decorations_.push_back(itemDef->decorations_[i]);
	}

	return newPanel;
}

UiWidgetPtr Ui::constructUiWidget(boost::shared_ptr<UiPanelElementDefinition> itemDef)
{
	std::string type = itemDef->type_;

	if (type == "")
	{
		std::cout << "No element type set." << std::endl;

		return nullptr;
	}

	boost::to_upper(type);

	std::string widgetTypeIdName = "UIWIDGET_" + type;

	itemDef->uiWidgetId_ = ids_->getIdFromName(widgetTypeIdName);

	if (itemDef->uiWidgetId_ == -1)
	{
		std::cout << "UI Widget type " << type << " does not exist." << std::endl;

		return nullptr;
	}

	UiWidgetPtr newWidget = uiWidgetFactory_->createUiWidgetBase(itemDef->panelElementType_, itemDef->name_);
	
	newWidget->debugger_ = debugger_;

	newWidget->scriptName_ = BaseIds::idScriptDataMap[itemDef->uiWidgetId_].getFileName();

	newWidget->scriptTypeName_ = BaseIds::idScriptDataMap[itemDef->uiWidgetId_].getScriptTypeName();
	
	newWidget->inputDeviceManager_ = inputDeviceManager_;

	newWidget->systemMessageDispatcher_ = systemMessageDispatcher_;

	newWidget->setTypeName(widgetTypeIdName);

	newWidget->buttonDownHandler_ = itemDef->buttonDownHandler_;

	newWidget->buttonUpHandler_ = itemDef->buttonUpHandler_;

	newWidget->selectElementHandler_ = itemDef->selectElementHandler_;

	newWidget->gotFocusHandler_ = itemDef->gotFocusHandler_;

	newWidget->lostFocusHandler_ = itemDef->lostFocusHandler_;

	newWidget->initializePythonData();

	newWidget->readParameters(itemDef->params_);

	newWidget->initialize();

	return newWidget;
}

void Ui::writePanelTree()
{
	std::string panelTree = uiPanelContainer_->getRoot()->getNameTree(0);

	debugger_->appendToLog(panelTree);

	debugger_->writeLogToFile();
}

void Ui::writePanelTreePy()
{
	PythonReleaseGil unlocker;

	writePanelTree();
}

void Ui::hidePanel(std::string panelName)
{
	boost::shared_ptr<UiCommandArgs> args = boost::make_shared<UiCommandArgs>(UiCommandArgs());

	args->elementName = panelName;

	UiCommand command;

	command.type = UI_COMMAND_HIDEPANEL;

	command.args = args;

	queuedCommands_.push_back(command);
}

void Ui::hidePanelInternal(std::string panelName)
{
	auto panel = getPanelByName(panelName);

	auto element = boost::static_pointer_cast<UiPanelElement>(panel);

	element->setIsVisible(false);

	elementHidden(element);
}

void Ui::hidePanelPy(std::string panelName)
{
	PythonReleaseGil unlocker;

	hidePanel(panelName);
}

void Ui::showPanel(std::string panelName)
{
	boost::shared_ptr<UiCommandArgs> args = boost::make_shared<UiCommandArgs>(UiCommandArgs());

	args->elementName = panelName;

	UiCommand command;

	command.type = UI_COMMAND_SHOWPANEL;

	command.args = args;

	queuedCommands_.push_back(command);
}

void Ui::showPanelInternal(std::string panelName)
{
	auto panel = getPanelByName(panelName);

	auto element = boost::static_pointer_cast<UiPanelElement>(panel);

	element->setIsVisible(true);

	elementShown(element);
}

void Ui::showPanelPy(std::string panelName)
{
	PythonReleaseGil unlocker;

	showPanel(panelName);
}

void Ui::hideWidget(std::string widgetName)
{
	auto widget = getWidgetByName(widgetName);

	auto element = boost::static_pointer_cast<UiPanelElement>(widget);

	element->setIsVisible(false);

	elementHidden(element);
}

void Ui::hideWidgetPy(std::string widgetName)
{
	PythonReleaseGil unlocker;

	hideWidget(widgetName);
}

void Ui::showWidget(std::string widgetName)
{
	auto widget = getWidgetByName(widgetName);

	auto element = boost::static_pointer_cast<UiPanelElement>(widget);

	element->setIsVisible(true);

	elementShown(element);
}

void Ui::showWidgetPy(std::string widgetName)
{
	PythonReleaseGil unlocker;

	showWidget(widgetName);
}

void Ui::panelVisibilityChanged(std::string elementName)
{
	auto panel = getPanelByName(elementName);

	auto element = boost::static_pointer_cast<UiPanelElement>(panel);

	if (element->isVisible_ == true)
	{
		elementShown(element);
	}
	else
	{
		elementHidden(element);
	}
}

void Ui::widgetVisibilityChanged(std::string elementName)
{
	auto widget = getWidgetByName(elementName);

	auto element = boost::static_pointer_cast<UiPanelElement>(widget);

	if (element->isVisible_ == true)
	{
		elementShown(element);
	}
	else
	{
		elementHidden(element);
	}
}