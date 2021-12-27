//#include "..\..\Headers\EngineCore\BufferWrapper.hpp"
//
//using namespace firemelon;
//
//BufferWrapper::BufferWrapper()
//{
//	bufferData_ = nullptr;
//	bufferSize_ = 0;
//}
//
//BufferWrapper::~BufferWrapper()
//{
//	if (bufferData_ != nullptr)
//	{
//		delete bufferData_;
//		bufferData_ = nullptr;
//	}
//}
//
//void BufferWrapper::allocateBuffer(unsigned int size)
//{
//	// Clear the buffer if it was previously allocated.
//	if (bufferData_ != nullptr)
//	{
//		delete bufferData_;
//		bufferData_ = nullptr;
//	}
//
//	bufferSize_ = size;
//
//	bufferData_ = new char[size];
//}
//
//void BufferWrapper::clearBuffer()
//{
//	// Clear the buffer if it was previously allocated.
//	if (bufferData_ != nullptr)
//	{
//		delete bufferData_;
//		bufferData_ = nullptr;
//	}
//
//	bufferSize_ = 0;
//}
//
//char BufferWrapper::operator[](std::size_t index)
//{
//	if (index >= 0 && index < bufferSize_)
//	{
//		return bufferData_[index];
//	}
//
//	return 0;
//}
//
//void BufferWrapper::setChar(std::size_t index, char data)
//{
//	if (index >= 0 && index < bufferSize_)
//	{
//		bufferData_[index] = data;
//	}
//}
//
//unsigned int BufferWrapper::getBufferSize()
//{
//	return bufferSize_;
//}
//
//char* BufferWrapper::getBuffer()
//{
//	return bufferData_;
//}