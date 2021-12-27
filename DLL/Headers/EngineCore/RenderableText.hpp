/* -------------------------------------------------------------------------
** RenderableText.hpp
**
** The RenderableText class is the generic base class from which user defined
** RenderableText subclasses are created. The user must implement the initialize,
** update, and render functions when deriving a child class from it. A RenderableText
** object represents a piece of text that is rendered on the screen at a given location.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */


#ifndef _RENDERABLETEXT_HPP_
#define _RENDERABLETEXT_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <vector>
#include <string>

#include "RenderableTextController.hpp"
#include "FontManager.hpp"
#include "ColorRgba.hpp"
#include "Debugger.hpp"
#include "PythonGil.hpp"

#include <boost/signals2.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/make_shared.hpp>

namespace firemelon
{
	typedef boost::shared_ptr<boost::signals2::signal<void (int, int)>> RenderableTextDestroySignal;
	typedef boost::signals2::signal<void (int, int)> RenderableTextDestroySignalRaw;
	
	class FIREMELONAPI RenderableText
	{
	public:
		friend class TextManager;

		RenderableText();
		virtual ~RenderableText();
		
		std::string		getDisplayTextPy();
		std::string		getDisplayText();
		
		void			setDisplayTextPy(std::string value);
		void			setDisplayText(std::string value);
		
		std::string		getFontNamePy();
		std::string		getFontName();

		void			setFontNamePy(std::string value);
		void			setFontName(std::string value);
		
		int				getXPy();
		int				getX();
		
		void			setXPy(int value);
		void			setX(int value);
		
		int				getYPy();
		int				getY();
		
		void			setYPy(int value);
		void			setY(int value);
			
		int				getLayerIndexPy();
		int				getLayerIndex();
		
		void			setLayerIndexPy(int value);
		void			setLayerIndex(int value);
		
		bool			getInterpolatePositionPy();
		bool			getInterpolatePosition();

		void			setInterpolatePositionPy(bool value);
		void			setInterpolatePosition(bool value);
		
		ColorRgbaPtr	getColorPy();
		ColorRgbaPtr	getColor();
				
		void			remove();

	protected:
		
		bool										getSuspendGameUpdates();
		void										setSuspendGameUpdates(bool value);

		boost::shared_ptr<FontManager>				getFontManager();
		
	private:

		void										preButtonDown(GameButtonId buttonId);
		virtual void								buttonDown(GameButtonId buttonId);

		void										preButtonUp(GameButtonId buttonId);
		virtual void								buttonUp(GameButtonId buttonId);

		void										cleanup();

		void										preInitialize();
		virtual void								initialize();
		
		void										preUpdate(double time);
		virtual void								update(double time);

		virtual void								render(int x, int y);
		
		DebuggerPtr									debugger_;

		boost::shared_ptr<FontManager>				fontManager_;

		// The text that will be rendered to the screen.
		std::string									displayText_;
		
		// The name of the sprite sheet that will be used as the font.
		std::string									fontName_;
		
		// The pixel position on the screen.
		int											x_;
		int											y_;

		// The previous pixel position on the screen. Used to interpolate when text needs to move
		// smoothly on the screen.
		int											previousX_;
		int											previousY_;

		// The index of the layer the text will be rendered on.
		int											layerIndex_;
		
		// The index of the room the text is in.
		int											roomIndex_;
		
		// The unique ID assigned to this renderable text object.
		int											id_;
		
		// Auto-incrementing static variable used to assign each entity a unique ID.
		static int									idCounter_;
		
		// If this is set it will interpolate the render position based on the previous position.
		bool										interpolatePosition_;

		// Indicates if this object was added from a python script. Python objects will deallocate
		// themselves, where as c++ objects need to be deleted explicitly.
		bool										isPyObject_;

		bool										isRemoved_;

		// The color of the text.
		ColorRgbaPtr								color_;

		boost::shared_ptr<RenderableTextController>	controller_;
		
		RenderableTextDestroySignal					renderableTextDestroySignal_;

		// Python functions.
		bool										implementsPyAdded_;
		PyObj										pyAdded_;

		bool										implementsPyButtonDown_;
		PyObj										pyButtonDown_;

		bool										implementsPyButtonUp_;
		PyObj										pyButtonUp_;

		bool										implementsPyInitialize_;
		PyObj										pyInitialize_;

		bool										implementsPyUpdate_;
		PyObj										pyUpdate_;

		bool										implementsPyRemoved_;
		PyObj										pyRemoved_;

		bool										implementsPyRender_;
		PyObj										pyRender_;
	};
}

#endif // _RENDERABLETEXT_HPP_