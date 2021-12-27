/* -------------------------------------------------------------------------
** Renderable.hpp
** 
** The Renderable class is the base class for anything that can be rendered
** via the render grid. Derived classes must implement the render method.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _RENDERABLE_HPP_
#define _RENDERABLE_HPP_

#include <boost/signals2.hpp>
#include <boost/shared_ptr.hpp>

#include <vector>

#include "Debugger.hpp"
#include "GridCell.hpp"
#include "GridBounds.hpp"
#include "Position.hpp"
#include "Renderer.hpp"
#include "EntityReplicationState.hpp"


namespace firemelon
{
	class Renderable
	{
	public:
		friend class RenderableCodeBehind;
		friend class RenderableManager;
		friend class Room;
		friend class StageController;

		Renderable();
		~Renderable();

		virtual void				initializeRenderable();

		virtual void				updateRenderable(double time);

		virtual void				render(double lerp);

		virtual void				renderDebugData(double lerp);

		virtual bool				getIsDynamic();
		
		virtual unsigned int		getHeight() = 0;
		virtual unsigned int		getWidth() = 0;
		virtual int 				getX() = 0;
		virtual int 				getY() = 0;

		//virtual bool				canReplicate() = 0;
		//virtual void				populateReplicationState(EntityReplicationState* replicationState) = 0;

		void						setPosition(boost::shared_ptr<Position> position);
		boost::shared_ptr<Position>	getPosition();

		int							getRenderableId();
		
		int							getGridCellStartRow();
		void						setGridCellStartRow(int value);

		int							getGridCellEndRow();
		void						setGridCellEndRow(int value);

		int							getGridCellStartCol();
		void						setGridCellStartCol(int value);

		int							getGridCellEndCol();
		void						setGridCellEndCol(int value);

		void						setMapLayer(int value);
		int							getMapLayer();
		
		void						setScaleFactor(float scaleFactor);
		float						getScaleFactor();

		void						setRenderOrder(int value);

		int							getRenderOrderPy();
		int							getRenderOrder();

		bool						getIsVisiblePy();
		bool						getIsVisible();

		void						setIsVisiblePy(bool isVisible);
		void						setIsVisible(bool isVisible);

		void						attachDebugger(DebuggerPtr debugger);
		void						attachRenderer(RendererPtr renderer);

		RenderEffectsPtr			getRenderEffects();

	protected:

		DebuggerPtr					getDebugger();
		boost::shared_ptr<Position>	getLayerPosition();
		RendererPtr					getRenderer();

		void						rendered(int x, int y);

		// Used by the renderable manager to ensure an entity only gets replicated once per frame.
		bool					doReplicate_;

		const RenderEffectsPtr	renderEffects_ = RenderEffectsPtr(new RenderEffects);

	private:
		
		// Auto-incrementing static variable used to assign each entity a unique ID.
		static int						idCounter_;
		
		int								id_;

		// The index of the layer this renderable is in.
		int								mapLayerIndex_;
		
		// Higher render orders will be rendered after lower render orders.
		int								renderOrder_;
		
		// Used by the renderable manager to ensure a renderable isn't rendered twice.
		bool							doRender_;

		// Whether the renderable should be rendered.
		bool							isVisible_;

		// The position of the layer that this renderable is on, relative to the camera.
		boost::shared_ptr<Position>		layerPosition_;

		// The position of the owner.
		boost::shared_ptr<Position>		position_;

		// The bounds of the visibility grid. Used to determine if this renderable is in a visible grid cell.
		boost::shared_ptr<GridBounds>	visibilityGridBounds_;
		
		// The render grid cell bounds this renderable occupies. Used so that the grid doesn't need to be searched.
		int								startRow_;
		int								endRow_;
		int								startCol_;
		int								endCol_;

		float							scaleFactor_;

		DebuggerPtr						debugger_;

		RendererPtr						renderer_;
		
		// Signals
		boost::shared_ptr<boost::signals2::signal<void(int, int)>>	renderedSignal_;
	};
	
	// Renderables with higher render orders will be rendered after those with lower orders.
	class RenderableRenderOrderCompare
	{
	public:
		bool operator() (boost::shared_ptr<Renderable> lhs, boost::shared_ptr<Renderable> rhs)
		{
			return lhs->getRenderOrder() < rhs->getRenderOrder();
		}
	};

	class RenderableIdCompare
	{
	public:
		bool operator() (boost::shared_ptr<Renderable> lhs, boost::shared_ptr<Renderable> rhs)
		{
			return lhs->getRenderableId() < rhs->getRenderableId();
		}
	};

	typedef boost::shared_ptr<Renderable> RenderablePtr;
	typedef std::vector<RenderablePtr> RenderablePtrList;

}

#endif // _RENDERABLE_HPP_