#include "..\..\Headers\EngineCore\RenderEffects.hpp"

using namespace firemelon;

RenderEffects::RenderEffects()
{
	alphaGradientRadialCenterPoint_ = boost::shared_ptr<Position>(new Position(0, 0));

	alphaGradientDirection_ = ALPHA_GRADIENT_NONE;

	alphaGradientFrom_ = 1.0f;

	alphaGradientRadius_ = 1.0f;

	alphaGradientTo_ = 1.0f;

	alphaMask_ = AlphaMaskPtr(new AlphaMask());

	blendColor_ = ColorRgbaPtr(new ColorRgba(1.0, 1.0, 1.0, 1.0));

	blendPercent_ = 0.0f;

	previousExtentLeft_ = -1.0;

	previousExtentRight_ = 1.0;

	previousExtentTop_ = -1.0;

	previousExtentBottom_ = 1.0;

	extentLeft_ = -1.0;

	extentRight_ = 1.0;

	extentTop_ = -1.0;

	extentBottom_ = 1.0;

	hueColor_ = ColorRgbaPtr(new ColorRgba(1.0, 1.0, 1.0, 1.0));

	interpolateExtents_ = false;
	
	interpolateRotation_ = false;

	mirrorHorizontal_ = false;

	outlineColor_ = ColorRgbaPtr(new ColorRgba(1.0, 1.0, 1.0, 0.0));

	scaleFactor_ = 1.0f;

	debug_ = false;
}

RenderEffects::~RenderEffects()
{
}

AlphaMaskPtr RenderEffects::getAlphaMaskPy()
{
	PythonReleaseGil unlocker;

	return getAlphaMask();
}

AlphaMaskPtr RenderEffects::getAlphaMask()
{
	return alphaMask_;
}

ColorRgbaPtr RenderEffects::getBlendColorPy()
{
	PythonReleaseGil unlocker;

	return getBlendColor();
}

ColorRgbaPtr RenderEffects::getBlendColor()
{
	return blendColor_;
}

float RenderEffects::getBlendPercentPy()
{
	PythonReleaseGil unlocker;

	return getBlendPercent();
}

float RenderEffects::getBlendPercent()
{
	return blendPercent_;
}

void RenderEffects::setBlendPercentPy(float blendPercent)
{
	PythonReleaseGil unlocker;

	setBlendPercent(blendPercent);
}

void RenderEffects::setBlendPercent(float blendPercent)
{
	blendPercent_ = blendPercent;
}

ColorRgbaPtr RenderEffects::getHueColorPy()
{
	PythonReleaseGil unlocker;

	return getHueColor();
}

ColorRgbaPtr RenderEffects::getHueColor()
{
	return hueColor_;
}


ColorRgbaPtr RenderEffects::getOutlineColorPy()
{
	PythonReleaseGil unlocker;

	return getOutlineColor();
}

ColorRgbaPtr RenderEffects::getOutlineColor()
{
	return outlineColor_;
}

bool RenderEffects::getMirrorHorizontalPy()
{
	PythonReleaseGil unlocker;

	return getMirrorHorizontal();
}

bool RenderEffects::getMirrorHorizontal()
{
	return mirrorHorizontal_;
}

void RenderEffects::setMirrorHorizontalPy(bool mirrorHorizontal)
{
	PythonReleaseGil unlocker;

	setMirrorHorizontal(mirrorHorizontal);
}

void RenderEffects::setMirrorHorizontal(bool mirrorHorizontal)
{
	mirrorHorizontal_ = mirrorHorizontal;
}

float RenderEffects::getExtentBottomPy()
{
	PythonReleaseGil unlocker;

	return getExtentBottom();
}

float RenderEffects::getExtentBottom()
{
	return extentBottom_;
}

float RenderEffects::getExtentLeftPy()
{
	PythonReleaseGil unlocker;

	return getExtentLeft();
}

float RenderEffects::getExtentLeft()
{
	return extentLeft_;
}

float RenderEffects::getExtentRightPy()
{
	PythonReleaseGil unlocker;

	return getExtentRight();
}

float RenderEffects::getExtentRight()
{
	return extentRight_;
}

float RenderEffects::getExtentTopPy()
{
	PythonReleaseGil unlocker;

	return getExtentTop();
}

float RenderEffects::getExtentTop()
{
	return extentTop_;
}

float RenderEffects::getPreviousExtentBottom()
{
	return previousExtentBottom_;
}

float RenderEffects::getPreviousExtentLeft()
{
	return previousExtentLeft_;
}

float RenderEffects::getPreviousExtentRight()
{
	return previousExtentRight_;
}

float RenderEffects::getPreviousExtentTop()
{
	return previousExtentTop_;
}

void RenderEffects::setExtentBottomPy(float extent)
{
	PythonReleaseGil unlocker;

	setExtentBottom(extent);
}

void RenderEffects::setExtentBottom(float extent)
{
	previousExtentBottom_ = extentBottom_;

	if (extent > 1.0f)
	{
		extentBottom_ = 1.0f;
	}
	else if (extent < -1.0f)
	{
		extentBottom_ = -1.0f;
	}
	else
	{
		extentBottom_ = extent;
	}
}

void RenderEffects::setExtentLeftPy(float extent)
{
	PythonReleaseGil unlocker;

	setExtentLeft(extent);
}

void RenderEffects::setExtentLeft(float extent)
{
	previousExtentLeft_ = extentLeft_;

	if (extent > 1.0f)
	{
		extentLeft_ = 1.0f;
	}
	else if (extent < -1.0f)
	{
		extentLeft_ = -1.0f;
	}
	else
	{
		extentLeft_ = extent;
	}
}

void RenderEffects::setExtentRightPy(float extent)
{
	PythonReleaseGil unlocker;

	setExtentRight(extent);
}

void RenderEffects::setExtentRight(float extent)
{
	previousExtentRight_ = extentRight_;

	if (extent > 1.0f)
	{
		extentRight_ = 1.0f;
	}
	else if (extent < -1.0f)
	{
		extentRight_ = -1.0f;
	}
	else
	{
		extentRight_ = extent;
	}
}

void RenderEffects::setExtentTopPy(float extent)
{
	PythonReleaseGil unlocker;

	setExtentTop(extent);
}

void RenderEffects::setExtentTop(float extent)
{
	previousExtentTop_ = extentTop_;

	if (extent > 1.0f)
	{
		extentTop_ = 1.0f;
	}
	else if (extent < -1.0f)
	{
		extentTop_ = -1.0f;
	}
	else
	{
		extentTop_ = extent;
	}
}

void RenderEffects::addRotationPy(RotationOperationPtr rotationOperation)
{
	PythonReleaseGil unlocker;

	addRotation(rotationOperation);
}

void RenderEffects::addRotation(RotationOperationPtr rotationOperation)
{
	rotationOperations_.push_back(rotationOperation);
}

void RenderEffects::clearRotations()
{
	rotationOperations_.clear();
}

RotationOperationPtr RenderEffects::getRotationOperationPy(int index)
{
	PythonReleaseGil unlocker;

	return getRotationOperation(index);
}

RotationOperationPtr RenderEffects::getRotationOperation(int index)
{
	if (debug_ == true)
	{
		bool debug = true;
	}

	return rotationOperations_[index];
}

int RenderEffects::getRotationOperationCountPy()
{
	PythonReleaseGil unlocker;

	return getRotationOperationCount();
}

int	RenderEffects::getRotationOperationCount()
{
	return rotationOperations_.size();
}

void RenderEffects::setAlphaGradientDirectionPy(AlphaGradientDirection alphaGradientDirection)
{
	PythonReleaseGil unlocker;

	setAlphaGradientDirection(alphaGradientDirection);
}

void RenderEffects::setAlphaGradientDirection(AlphaGradientDirection alphaGradientDirection)
{
	alphaGradientDirection_ = alphaGradientDirection;
}

AlphaGradientDirection RenderEffects::getAlphaGradientDirectionPy()
{
	PythonReleaseGil unlocker;

	return getAlphaGradientDirection();
}

AlphaGradientDirection RenderEffects::getAlphaGradientDirection()
{
	return alphaGradientDirection_;
}

void RenderEffects::setAlphaGradientFromPy(float alpha)
{
	PythonReleaseGil unlocker;

	setAlphaGradientFrom(alpha);
}

void RenderEffects::setAlphaGradientFrom(float alpha)
{
	if (alpha > 1.0f)
	{
		alpha = 1.0f;
	}
	else if(alpha < 0.0f)
	{
		alpha = 0.0f;
	}

	alphaGradientFrom_ = alpha;
}

float RenderEffects::getAlphaGradientFromPy()
{
	PythonReleaseGil unlocker;

	return getAlphaGradientFrom();
}

float RenderEffects::getAlphaGradientFrom()
{
	return alphaGradientFrom_;
}


void RenderEffects::setAlphaGradientToPy(float alpha)
{
	PythonReleaseGil unlocker;

	setAlphaGradientTo(alpha);
}

void RenderEffects::setAlphaGradientTo(float alpha)
{
	if (alpha > 1.0f)
	{
		alpha = 1.0f;
	}
	else if (alpha < 0.0f)
	{
		alpha = 0.0f;
	}

	alphaGradientTo_ = alpha;
}

float RenderEffects::getAlphaGradientToPy()
{
	PythonReleaseGil unlocker;

	return getAlphaGradientTo();
}

float RenderEffects::getAlphaGradientTo()
{
	return alphaGradientTo_;
}


void RenderEffects::setAlphaGradientRadiusPy(float radius)
{
	PythonReleaseGil unlocker;

	setAlphaGradientRadius(radius);
}

void RenderEffects::setAlphaGradientRadius(float radius)
{
	alphaGradientRadius_ = radius;
}

float RenderEffects::getAlphaGradientRadiusPy()
{
	PythonReleaseGil unlocker;

	return getAlphaGradientRadius();
}

float RenderEffects::getAlphaGradientRadius()
{
	return alphaGradientRadius_;
}

boost::shared_ptr<Position> RenderEffects::getAlphaGradientRadialCenterPointPy()
{
	PythonReleaseGil unlocker;

	return getAlphaGradientRadialCenterPoint();
}

boost::shared_ptr<Position> RenderEffects::getAlphaGradientRadialCenterPoint()
{
	return alphaGradientRadialCenterPoint_;
}

float RenderEffects::getScaleFactorPy()
{
	PythonReleaseGil unlocker;

	return getScaleFactor();
}

float RenderEffects::getScaleFactor()
{
	return scaleFactor_;
}

void RenderEffects::setScaleFactorPy(float scaleFactor)
{
	PythonReleaseGil unlocker;

	setScaleFactor(scaleFactor);
}

void RenderEffects::setScaleFactor(float scaleFactor)
{
	scaleFactor_ = scaleFactor;
}

bool RenderEffects::getInterpolateExtentsPy()
{
	PythonReleaseGil unlocker;

	return getInterpolateExtents();
}

bool RenderEffects::getInterpolateExtents()
{
	return interpolateExtents_;
}

void RenderEffects::setInterpolateExtentsPy(bool interpolateExtents)
{
	PythonReleaseGil unlocker;

	setInterpolateExtents(interpolateExtents);
}

void RenderEffects::setInterpolateExtents(bool interpolateExtents)
{
	interpolateExtents_ = interpolateExtents;
}

bool RenderEffects::getInterpolateRotationPy()
{
	PythonReleaseGil unlocker;

	return getInterpolateRotation();
}

bool RenderEffects::getInterpolateRotation()
{
	return interpolateRotation_;
}

void RenderEffects::setInterpolateRotationPy(bool interpolateRotation)
{
	PythonReleaseGil unlocker;

	setInterpolateRotation(interpolateRotation);
}

void RenderEffects::setInterpolateRotation(bool interpolateRotation)
{
	interpolateRotation_ = interpolateRotation;
}
