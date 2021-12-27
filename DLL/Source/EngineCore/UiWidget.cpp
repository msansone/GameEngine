#include "..\..\Headers\EngineCore\UiWidget.hpp"

using namespace firemelon;
using namespace boost::python;

int UiWidget::idCounter_ = 0;

UiWidget::UiWidget(std::string widgetName)
{
	isVisible_ = true;

	isEnabled_ = true;
	
	name_ = widgetName;

	typeName_ = "";

	elementType_ = PANEL_ELEMENT_WIDGET;

	// Increment the ID to be used in the python instance name.
	id_ = idCounter_;

	idCounter_++;
}

UiWidget::~UiWidget()
{
}

bool UiWidget::getIsFocusable()
{
	return focusable_;
}


void UiWidget::setTypeName(std::string typeName)
{
	typeName_ = typeName;
}

void UiWidget::setTypeId(UiWidgetId typeId)
{
	typeId_ = typeId;
}

boost::python::object UiWidget::getPyInstance()
{
	return pyUiWidgetInstance_;
}

boost::shared_ptr<InputDeviceManager> UiWidget::getInputDeviceManager()
{
	return inputDeviceManager_;
}

std::string UiWidget::getScriptVar()
{
	return scriptVar_;
}

std::string UiWidget::getScriptName()
{
	return scriptName_;
}

std::string	UiWidget::getScriptTypeName()
{
	return scriptTypeName_;
}

void UiWidget::initializePythonData()
{
	PythonAcquireGil lock;

	try
	{
		std::string sCode = "from ";

		scriptVar_ = scriptTypeName_ + boost::lexical_cast<std::string>(id_);
		std::replace(scriptVar_.begin(), scriptVar_.end(), ' ', '_');

		sCode += scriptName_ + " import " + scriptTypeName_ + "\n";
		sCode += scriptVar_ + " = " + scriptTypeName_ + "()";

		pyMainModule_ = import("__main__");
		pyMainNamespace_ = pyMainModule_.attr("__dict__");

		boost::python::str pyCode(sCode);
		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);

		// Get the instance for this object.
		pyUiWidgetInstance_ = extract<object>(pyMainNamespace_[scriptVar_]);
		pyUiWidgetNamespace_ = pyUiWidgetInstance_.attr("__dict__");

		// Import firemelon module to the instance.
		object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
		pyUiWidgetNamespace_["firemelon"] = pyFiremelonModule;

		// Store the functions as python objects.
		pyInitialize_ = pyUiWidgetInstance_.attr("initialize");
		pyCalculateSize_ = pyUiWidgetInstance_.attr("calculateSize");
		pyUpdate_ = pyUiWidgetInstance_.attr("update");
		pyRender_ = pyUiWidgetInstance_.attr("render");
		pyButtonDown_ = pyUiWidgetInstance_.attr("buttonDown");
		pyButtonUp_ = pyUiWidgetInstance_.attr("buttonUp");
		pyEnter_ = pyUiWidgetInstance_.attr("enter");
		pyLeave_ = pyUiWidgetInstance_.attr("leave");
		pyReadParameters_ = pyUiWidgetInstance_.attr("readParameters");


		pyUiWidgetInstance_.attr("isVisisble") = isVisible_;		
		pyUiWidgetInstance_.attr("isEnabled") = isEnabled_;
		pyUiWidgetInstance_.attr("hasFocus") = hasFocus_;
		pyUiWidgetInstance_.attr("size") = getSize();
		pyUiWidgetInstance_.attr("position") = getPosition();
				
		// If this menu item implements the systemMessageReceived function, add it as a system message listener.		
		if (PyObject_HasAttrString(pyUiWidgetInstance_.ptr(), "systemMessageReceived") == true)
		{
			systemMessageDispatcher_->addListener(pyUiWidgetInstance_);
		}

		pythonDataInitialized();
	}
	catch (error_already_set &)
	{
		std::cout << "Error loading menu item " + scriptTypeName_ << std::endl;
		debugger_->handlePythonError();
	}
}

void UiWidget::cleanupPythonData()
{
	try
	{
		PythonAcquireGil lock;

		pyUiWidgetNamespace_ = boost::python::object();
		pyUiWidgetInstance_ = boost::python::object();
		pyInitialize_ = boost::python::object();
		pyCalculateSize_ = boost::python::object();
		pyUpdate_ = boost::python::object();
		pyRender_ = boost::python::object();
		pyButtonDown_ = boost::python::object();
		pyButtonUp_ = boost::python::object();
		pyEnter_ = boost::python::object();
		pyLeave_ = boost::python::object();
		pyReadParameters_ = boost::python::object();

		std::string sCode = scriptVar_ + " = None";

		boost::python::str pyCode(sCode);

		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);
	}
	catch (error_already_set &)
	{
		std::cout << "Error deleting UI widget script." << std::endl;
		debugger_->handlePythonError();
	}
}

void UiWidget::render()
{
	PythonAcquireGil lock;

	try
	{
		pyRender_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void UiWidget::initialize()
{
	PythonAcquireGil lock;

	try
	{
		pyInitialize_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void UiWidget::calculateMinimumBoundingSize()
{
	calculateSize();
	
	calculateRectSizes(getSize()->getWidth(), getSize()->getHeight(), 0, 0);
}

SizePtr UiWidget::getFullSize()
{
	calculateSize();

	return getSize();
}

void UiWidget::calculateSize()
{
	PythonAcquireGil lock;

	try
	{
		pyCalculateSize_();

	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void UiWidget::readParameters(std::string parameters)
{
	PythonAcquireGil lock;

	try
	{
		pyReadParameters_(parameters);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void UiWidget::update(double time)
{
	PythonAcquireGil lock;

	try
	{
		pyUpdate_(time);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void UiWidget::calculateMinimumBoundingSizeBase()
{
	calculateMinimumBoundingSize();
}

void UiWidget::locateElementsBase()
{
	// Widgets don't contain elements. Do nothing.
}

void UiWidget::locateChildElements()
{
	// Widgets don't contain elements. Do nothing.
}

void UiWidget::renderBackground(int parentX, int parentY)
{
	// Fill in the background.
	int left = rects_->paddingOuterBoundary->x + parentX - backgroundExtensionLeft_;

	int right = left + rects_->paddingOuterBoundary->w + backgroundExtensionLeft_ + backgroundExtensionRight_;

	int top = rects_->paddingOuterBoundary->y + parentY - backgroundExtensionTop_;

	int bottom = top + rects_->paddingOuterBoundary->h + backgroundExtensionTop_ + backgroundExtensionBottom_;

	if (backgroundSheetName_ != "")
	{
		RendererPtr renderer = getRenderer();

		// Render the background
		int backgroundSurfaceId = renderer->getSheetIDByName(backgroundSheetName_);

		boost::shared_ptr<SpriteSheet> sheet = renderer->getSheet(backgroundSurfaceId);

		int backgroundCellHeight = (sheet->getCellHeight() * sheet->getScaleFactor());

		int backgroundCellWidth = (sheet->getCellWidth() * sheet->getScaleFactor());
		
		// Fill in the background
		for (int i = top; i < bottom; i += backgroundCellHeight)
		{
			for (int j = left; j < right; j += backgroundCellWidth)
			{
				backgroundRenderEffects_->setExtentRight(1.0f);
				backgroundRenderEffects_->setExtentBottom(1.0f);

				if (j + backgroundCellWidth > right)
				{
					// The background is going to extend too far. Set the extent accordingly.
					int extraWidth = (j + backgroundCellWidth) - right;

					int dispayWidth = backgroundCellWidth - extraWidth;

					float extentPercent = ((float)dispayWidth / (float)backgroundCellWidth) * 100.0f;

					// Transform the percent to a range of - 50 to 50, and the divide by 50 to transform it 
					// into a range - 1 to 1, which is what the extent uses.
					float extentRight = ((float)(extentPercent - 50.0f) / 50.0f);

					backgroundRenderEffects_->setExtentRight(extentRight);
				}

				if (i + backgroundCellHeight > bottom)
				{
					// The background is going to extend too far. Set the extent accordingly.
					int extraHeight = (i + backgroundCellHeight) - bottom;

					int dispayHeight = backgroundCellHeight - extraHeight;

					float extentPercent = ((float)dispayHeight / (float)backgroundCellHeight) * 100.0f;

					// Transform the percent to a range of - 50 to 50, and the divide by 50 to transform it 
					// into a range - 1 to 1, which is what the extent uses.
					float extentBottom = ((float)(extentPercent - 50.0f) / 50.0f);

					backgroundRenderEffects_->setExtentBottom(extentBottom);
				}

				renderer->renderSheetCell(j, i, backgroundSurfaceId, 1, 1, backgroundRenderEffects_);
			}
		}

		backgroundRenderEffects_->setExtentRight(1.0f);
		backgroundRenderEffects_->setExtentBottom(1.0f);

		// Fill in the top and bottom borders.
		for (int j = left; j < right; j += backgroundCellWidth)
		{
			if (j + backgroundCellWidth > right)
			{
				// The background is going to extend too far. Set the extent accordingly.
				int extraWidth = (j + backgroundCellWidth) - (right);

				int dispayWidth = backgroundCellWidth - extraWidth;

				float extentPercent = ((float)dispayWidth / (float)backgroundCellWidth) * 100.0f;

				// Transform the percent to a range of - 50 to 50, and the divide by 50 to transform it 
				// into a range - 1 to 1, which is what the extent uses.
				float extentRight = ((float)(extentPercent - 50.0f) / 50.0f);

				backgroundRenderEffects_->setExtentRight(extentRight);
			}

			renderer->renderSheetCell(j, top, backgroundSurfaceId, 1, 0, backgroundRenderEffects_);

			renderer->renderSheetCell(j, bottom - backgroundCellHeight, backgroundSurfaceId, 1, 2, backgroundRenderEffects_);
		}

		backgroundRenderEffects_->setExtentRight(1.0f);

		// Then, fill in the left and right borders.
		for (int i = top; i < bottom; i += backgroundCellHeight)
		{
			if (i + backgroundCellHeight > bottom)
			{
				// The background is going to extend too far. Set the extent accordingly.
				int extraHeight = (i + backgroundCellHeight) - (bottom);

				int dispayHeight = backgroundCellHeight - extraHeight;

				float extentPercent = ((float)dispayHeight / (float)backgroundCellHeight) * 100.0f;

				// Transform the percent to a range of - 50 to 50, and the divide by 50 to transform it 
				// into a range - 1 to 1, which is what the extent uses.
				float extentBottom = ((float)(extentPercent - 50.0f) / 50.0f);

				backgroundRenderEffects_->setExtentBottom(extentBottom);
			}

			// Render the left border tile.
			renderer->renderSheetCell(left, i, backgroundSurfaceId, 0, 1, backgroundRenderEffects_);

			// Render the right border tile.
			renderer->renderSheetCell(right - backgroundCellWidth, i, backgroundSurfaceId, 2, 1, backgroundRenderEffects_);
		}

		// Render the top left corner.
		renderer->renderSheetCell(left, top, backgroundSurfaceId, 0, 0);

		// Render the top right corner.
		renderer->renderSheetCell(right - backgroundCellHeight, top, backgroundSurfaceId, 2, 0);

		// Render the bottom right corner.
		renderer->renderSheetCell(left, bottom - backgroundCellHeight, backgroundSurfaceId, 0, 2);

		// Render the bottom left corner.
		renderer->renderSheetCell(right - backgroundCellHeight, bottom - backgroundCellHeight, backgroundSurfaceId, 2, 2);

	}
}

void UiWidget::renderBase(int parentX, int parentY)
{
	if (isVisible_ == true)
	{
		renderBackground(parentX, parentY);

		getPosition()->setX(rects_->paddingInnerBoundary->x + parentX);

		getPosition()->setY(rects_->paddingInnerBoundary->y + parentY);

		render();

		if (debugger_->getDebugMode() == true && (debugger_->getPanelElementName() == getName() || debugger_->getPanelElementName() == "all"))
		{
			RendererPtr renderer = getRenderer();

			renderer->drawRect(rects_->paddingOuterBoundary->x + parentX, rects_->paddingOuterBoundary->y + parentY, rects_->paddingOuterBoundary->w, rects_->paddingOuterBoundary->h, 1.0, 0.0, 0.0, 1.0);

			renderer->drawRect(rects_->paddingInnerBoundary->x + parentX, rects_->paddingInnerBoundary->y + parentY, rects_->paddingInnerBoundary->w, rects_->paddingInnerBoundary->h, 1.0, 0.6, 0.2, 1.0);
		}
	}
}

void UiWidget::updateBase(double time)
{
	shownEventCalled_ = false;
	hiddenEventCalled_ = false;

	hasFocus_ = (indexInParent_ == parentPanel_->focusedChildIndex_) && parentPanel_->hasFocus_;

	// Update the flags in the pyhton instance.
	{
		PythonAcquireGil lock;

		pyUiWidgetInstance_.attr("isVisible") = isVisible_;

		pyUiWidgetInstance_.attr("isEnabled") = isEnabled_;

		pyUiWidgetInstance_.attr("hasFocus") = hasFocus_;
	}

	if (hasFocus_ == true)
	{
		update(time);
	}
}

void UiWidget::buttonDown(GameButtonId buttonCode)
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

void UiWidget::buttonUp(GameButtonId buttonCode)
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

void UiWidget::created()
{
	// Widgets default to visible.
	isVisible_ = true;
}

void UiWidget::pythonDataInitialized()
{
	return;
}

void UiWidget::reset()
{

}