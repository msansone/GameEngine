/* -------------------------------------------------------------------------
** RenderEffects.hpp
**
** The RenderEffects class is used to set different effects that can be applied
** when rendering an image in the renderer, such as color hue, rotation, or
** mirroring.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _RENDEREFFECTS_HPP_
#define _RENDEREFFECTS_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "AlphaMask.hpp"
#include "ColorRgba.hpp"
#include "RotationOperation.hpp"

namespace firemelon
{
	enum AlphaGradientDirection
	{
		ALPHA_GRADIENT_NONE = 0,
		ALPHA_GRADIENT_NORTH = 1,
		ALPHA_GRADIENT_SOUTH = 2,
		ALPHA_GRADIENT_EAST = 3,
		ALPHA_GRADIENT_WEST = 4,
		ALPHA_GRADIENT_NORTHEAST = 5,
		ALPHA_GRADIENT_NORTHWEST = 6,
		ALPHA_GRADIENT_SOUTHEAST = 7,
		ALPHA_GRADIENT_SOUTHWEST = 8,
		ALPHA_GRADIENT_RADIAL = 9
	};

	class FIREMELONAPI RenderEffects
	{
	public:
		friend class StageMetadata;
		friend class StageRenderable;

		RenderEffects();
		virtual ~RenderEffects();

		void					addRotationPy(RotationOperationPtr rotationOperation);
		void					addRotation(RotationOperationPtr rotationOperation);

		void					clearRotations();

		AlphaGradientDirection	getAlphaGradientDirectionPy();
		AlphaGradientDirection	getAlphaGradientDirection();

		float					getAlphaGradientFromPy();
		float					getAlphaGradientFrom();

		float					getAlphaGradientToPy();
		float					getAlphaGradientTo();

		PositionPtr				getAlphaGradientRadialCenterPointPy();
		PositionPtr				getAlphaGradientRadialCenterPoint();

		float					getAlphaGradientRadiusPy();
		float					getAlphaGradientRadius();
		
		AlphaMaskPtr			getAlphaMaskPy();
		AlphaMaskPtr			getAlphaMask();

		ColorRgbaPtr			getBlendColorPy();
		ColorRgbaPtr			getBlendColor();

		float					getBlendPercentPy();
		float					getBlendPercent();

		float					getExtentBottomPy();
		float					getExtentBottom();
		float					getExtentLeftPy();
		float					getExtentLeft();
		float					getExtentRightPy();
		float					getExtentRight();
		float					getExtentTopPy();
		float					getExtentTop();

		bool					getInterpolateExtentsPy();
		bool					getInterpolateExtents();

		bool					getInterpolateRotationPy();
		bool					getInterpolateRotation();

		ColorRgbaPtr			getHueColorPy();
		ColorRgbaPtr			getHueColor();

		ColorRgbaPtr			getOutlineColorPy();
		ColorRgbaPtr			getOutlineColor();

		bool					getMirrorHorizontalPy();
		bool					getMirrorHorizontal();

		RotationOperationPtr	getRotationOperationPy(int index);
		RotationOperationPtr	getRotationOperation(int index);

		int						getRotationOperationCountPy();
		int						getRotationOperationCount();

		float					getScaleFactorPy();
		float					getScaleFactor();

		void					setAlphaGradientDirectionPy(AlphaGradientDirection alphaGradientDirection);
		void					setAlphaGradientDirection(AlphaGradientDirection alphaGradientDirection);

		void					setAlphaGradientFromPy(float alpha);
		void					setAlphaGradientFrom(float alpha);

		void					setAlphaGradientToPy(float alpha);
		void					setAlphaGradientTo(float alpha);

		void					setAlphaGradientRadiusPy(float alpha);
		void					setAlphaGradientRadius(float alpha);

		void					setBlendPercentPy(float blendPercent);
		void					setBlendPercent(float blendPercent);

		void					setExtentBottomPy(float extent);
		void					setExtentBottom(float extent);
		void					setExtentLeftPy(float extent);
		void					setExtentLeft(float extent);
		void					setExtentRightPy(float extent);
		void					setExtentRight(float extent);
		void					setExtentTopPy(float extent);
		void					setExtentTop(float extent);

		void					setInterpolateExtentsPy(bool interpolateExtents);
		void					setInterpolateExtents(bool interpolateExtents);

		void					setInterpolateRotationPy(bool interpolateRotation);
		void					setInterpolateRotation(bool interpolateRotation);

		void					setMirrorHorizontalPy(bool mirrorHorizontal);
		void					setMirrorHorizontal(bool mirrorHorizontal);

		void					setScaleFactorPy(float scaleFactor);
		void					setScaleFactor(float scaleFactor);

	private:

		float	getPreviousExtentBottom();
		float	getPreviousExtentLeft();
		float	getPreviousExtentRight();
		float	getPreviousExtentTop();

		AlphaGradientDirection		alphaGradientDirection_;

		float						alphaGradientFrom_;

		PositionPtr					alphaGradientRadialCenterPoint_;

		float						alphaGradientRadius_;

		float						alphaGradientTo_;

		AlphaMaskPtr				alphaMask_;

		ColorRgbaPtr				blendColor_;

		float						blendPercent_;

		float						extentLeft_;

		float						extentRight_;

		float						extentTop_;

		float						extentBottom_;

		ColorRgbaPtr				hueColor_;

		bool						interpolateExtents_;

		bool						interpolateRotation_;
		
		bool						mirrorHorizontal_;

		ColorRgbaPtr				outlineColor_;

		float						previousExtentLeft_;

		float						previousExtentRight_;

		float						previousExtentTop_;

		float						previousExtentBottom_;

		RotationOperationPtrList	rotationOperations_;

		float						scaleFactor_;

		bool						debug_;
	};

	typedef boost::shared_ptr<RenderEffects>	RenderEffectsPtr;
	typedef std::vector<RenderEffectsPtr>		RenderEffectsPtrList;
}

#endif // _RENDEREFFECTS_HPP_
