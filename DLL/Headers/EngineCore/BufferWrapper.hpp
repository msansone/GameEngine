///* -------------------------------------------------------------------------
//** BufferWrapper.hpp
//**
//** The BufferWrapper class is a wrapper around a byte array. It use used when
//** receiving a serialized entity, and storing it's data to be replicated at a
//** later time.
//**
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _BUFFERWRAPPER_HPP_
//#define _BUFFERWRAPPER_HPP_
//
//#include <iostream>
//
//namespace firemelon
//{
//	class BufferWrapper
//	{
//	public:
//
//		BufferWrapper();
//		virtual ~BufferWrapper();
//		
//		void			allocateBuffer(unsigned int size);
//		void			clearBuffer();
//
//		unsigned int	getBufferSize();		
//		char*			getBuffer();
//		
//		void			setChar(std::size_t index, char data);
//
//		char			operator[](std::size_t index);
//
//	private:
//		
//		unsigned int	bufferSize_;
//		char*			bufferData_;
//	};
//}
//
//#endif // _BUFFERWRAPPER_HPP_