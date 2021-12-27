/* -------------------------------------------------------------------------
** TileRenderable.hpp
**
** The TileRenderable class is the renderable component for a tile. Tiles
** used to use state machine controllers, but they contained a lot of unnecessary
** overhead that were causing a performance hit, given that many tiles must
** be rendered every frame. TileRenderables should contain as little code
** as possible in the implementation of the render function.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _TILERENDERABLE_HPP_
#define _TILERENDERABLE_HPP_

#include "Debugger.hpp"
#include "Renderable.hpp"

namespace firemelon
{
	class TileRenderable : public Renderable
	{
	public:

		TileRenderable();
		virtual ~TileRenderable();

		void			setFramesPerSecond(int framesPerSecond);
		void			setHeight(int height);
		void			setTileSheetColumn(int tileSheetColumn);
		void			setTileSheetId(int tileSheetId);
		void			setTileSheetRegions(std::vector<Rect> regions);
		void			setTileSheetRow(int tileSheetRow);
		void			setWidth(int width);

	private:

		// Renderable virtual functions.
		void			initializeRenderable();
		void			render(double lerp);
		unsigned int	getHeight();
		unsigned int	getWidth();
		int 			getX();
		int 			getY();

		void			updateRenderable(double time);

		//bool			canReplicate();
		//void			populateReplicationState(EntityReplicationState* replicationState);

		int							height_;
		PositionPtr					layerPosition_;
		PositionPtr					position_;
		int							regionIndex_;
		RendererPtr					renderer_;
		int							tileSheetColumn_;
		int							tileSheetId_;
		std::vector<Rect>			tileSheetRegions_;
		std::vector<Rect>			tileSheetRegionsScaled_;
		int							tileSheetRow_;
		double						updateInterval_;
		double						updateTimer_;
		bool						useRegion_;
		int							width_;
	};
}

#endif // _TILERENDERABLE_HPP_