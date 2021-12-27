/* -------------------------------------------------------------------------
** TextManager.hpp
**
** The TextManager class stores and managers instances of the RenderText class
** which describe holds instances of text that are to be rendered on screen. 
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _TEXTMANAGER_HPP_
#define _TEXTMANAGER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <string>
#include <vector>
#include <map>

#include <boost/algorithm/string.hpp>

#include "Renderer.hpp"
#include "AudioPlayer.hpp"
#include "InputDeviceManager.hpp"
#include "BitmapFont.hpp"
#include "Position.hpp"
#include "BasicRenderableText.hpp"

namespace firemelon
{
	class FIREMELONAPI TextManager
	{
	public:
		friend class GameEngine;
		friend class RenderableText;
		friend class RoomContainer;

		TextManager(RendererPtr renderer, InputDeviceManagerPtr inputDeviceManager, FontManagerPtr fontManager, DebuggerPtr debugger);
		virtual ~TextManager();
	
		void															initialize();	
		void															preUpdate(int roomIndex);
		void															update(int roomIndex, double time);
		void															render(int roomIndex, double lerp);	
		
		void															addRoom(RoomId roomId);
		void															addText(RoomId roomId, boost::shared_ptr<RenderableText> renderableText);
		void															addTextPy(RoomId roomId, PyObj pyRenderableText);
		
		void															addLayerPosition(int roomIndex, boost::shared_ptr<Position> pos);
		
	private:
		
		void															buttonDown(boost::shared_ptr<InputEvent> inputEvent);
		void															buttonUp(boost::shared_ptr<InputEvent> inputEvent);
		void															changeInputChannel(InputChannel oldInputChannel, InputChannel newInputChannel);
		void															cleanup();
		void															renderableTextDestroyed(int roomIndex, int id);
		
		DebuggerPtr														debugger_;
		FontManagerPtr													fontManager_;
		InputDeviceManagerPtr											inputDeviceManager_;
		RendererPtr														renderer_;
		std::map<firemelon::RoomId, int>								roomIdToIndexMap_;
		std::vector<std::vector<boost::shared_ptr<RenderableText>>>		textList_;
		
		// An array of layer positions relative to the camera.
		// Used for rendering text on a layer, rather than a screen location.
		std::vector<std::vector<boost::shared_ptr<Position>>>			layerPositions_;
		
		InputChannel													inputChannel_;

		RoomId															showingRoomId_;
	};
}

#endif // _TEXTMANAGER_HPP_