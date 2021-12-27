/* -------------------------------------------------------------------------
** BasicRenderableText.hpp
**
** The BasicRenderableText class is derived from the RenderableText class. It
** provides a simple object whichis used to render a string to the screen.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _BASICRENDERABLETEXT_HPP_
#define _BASICRENDERABLETEXT_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "RenderableText.hpp"

namespace firemelon
{
	class FIREMELONAPI BasicRenderableText : public RenderableText
	{
	public:

		BasicRenderableText();
		virtual ~BasicRenderableText();

	private:
		
		virtual void	initialize();
		
		virtual void	update(double time);

		virtual void	render(int x, int y);

		boost::shared_ptr<BitmapFont>	font_;
	};
}

#endif // _BASICRENDERABLETEXT_HPP_