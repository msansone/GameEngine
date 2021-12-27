/* -------------------------------------------------------------------------
** ServerLayer.hpp
**
** The ServerLayer class stores the server side data of the network layer.
** 
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SERVERLAYER_HPP_
#define _SERVERLAYER_HPP_

#include <boost/signals2.hpp>

#include "InputState.hpp"

namespace firemelon
{
	typedef boost::signals2::signal<void (std::vector<GameButtonId>, std::vector<ButtonState>, std::string)> InputReceivedSignal;

	class ServerLayer
	{
	public:
		
		ServerLayer();
		virtual ~ServerLayer();

		void initialize();
		
		InputReceivedSignal	inputReceivedFromClientSignal_;
	};
}

#endif // _SERVERLAYER_HPP_