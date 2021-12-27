/* -------------------------------------------------------------------------
** AnimationFrame.hpp
** 
** The AnimationFrame class points to a cell in a sprite sheet, and contains
** a list of hitbox references that become active when the frame is displayed.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ANIMATIONFRAME_HPP_
#define _ANIMATIONFRAME_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <map>
#include <vector>

#include "AnchorPointManager.hpp"
#include "BaseIds.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI AnimationFrame
	{
	public:
		friend class Assets;
		friend class StageRenderable;

		AnimationFrame(int x, int y);
		virtual ~AnimationFrame();
	
		void			addHitboxReferencePy(int id);
		void			addHitboxReference(int id);

		void			addAnchorPointReferencePy(int id);
		void			addAnchorPointReference(int id);

		void			addTriggerSignalPy(TriggerSignalId triggerSignalId);
		void			addTriggerSignal(TriggerSignalId triggerSignalId);
		
		int				getSpriteSheetCellColumnPy();
		int				getSpriteSheetCellColumn();
		int				getSpriteSheetCellRowPy();
		int				getSpriteSheetCellRow();
		
		int				getHitboxCountPy();
		int				getHitboxCount();

		int				getHitboxReferencePy(int index);
		int				getHitboxReference(int index);
		
		int				getAnchorPointCountPy();
		int				getAnchorPointCount();

		int				getAnchorPointReferencePy(int index);
		int				getAnchorPointReference(int index);

		AnchorPointPtr	getAnchorPointPy(int index);
		AnchorPointPtr	getAnchorPoint(int index);

		TriggerSignalId	getTriggerSignalCountPy();
		TriggerSignalId	getTriggerSignalCount();
		TriggerSignalId	getTriggerSignalPy(int index);
		TriggerSignalId	getTriggerSignal(int index);

		void			setAlphaMaskSheetCellColumn(int value);
		void			setAlphaMaskSheetCellRow(int value);

	private:

		int										alphaMaskSheetCellColumn_;
		int										alphaMaskSheetCellRow_;

		int										spriteSheetCellColumn_; 
		int										spriteSheetCellRow_; 
		
		std::vector<int>						hitboxReferences_;
		std::vector<int>						anchorPointReferences_;
		std::vector<TriggerSignalId>			triggerSignals_;

		boost::shared_ptr<AnchorPointManager>	anchorPointManager_;
	};

	typedef boost::shared_ptr<AnimationFrame> AnimationFramePtr;
}

#endif // _ANIMATIONFRAME_HPP_