#ifndef _FIREMELONEXCODEBEHINDFACTORY_HPP_
#define _FIREMELONEXCODEBEHINDFACTORY_HPP_

#include <boost/shared_ptr.hpp>

#include <CodeBehindFactory.hpp>
#include "FiremelonExEntityCodeBehind.hpp"
#include "FiremelonExInputReceiverCodeBehind.hpp"
#include "SdlKeyboardDevice.hpp"

class FiremelonExCodeBehindFactory : public firemelon::CodeBehindFactory
{
public:
	FiremelonExCodeBehindFactory();
	virtual ~FiremelonExCodeBehindFactory();

	void															attachKeyboardDevice(boost::shared_ptr<SdlKeyboardDevice> keyboardDevice);

protected:

	virtual boost::shared_ptr<firemelon::EntityCodeBehind>			createEntityCodeBehind();
	virtual boost::shared_ptr<firemelon::InputReceiverCodeBehind>	createInputReceiverCodeBehind();

private:

	boost::shared_ptr<SdlKeyboardDevice>							keyboardDevice_;
};

#endif // _FIREMELONEXCODEBEHINDFACTORY_HPP_