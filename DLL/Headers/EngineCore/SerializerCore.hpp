///* -------------------------------------------------------------------------
//** SerializerCore.hpp
//** 
//** The SerializerCore class is used to serialize entity state data to and
//** from a byte array. It is used to simplify the serialization process especially
//** when being used inside a python script.
//**
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _SERIALIZERCORE_HPP_
//#define _SERIALIZERCORE_HPP_
//
//#include <vector>
//#include <boost/shared_ptr.hpp>
//
//#include "NetworkUtility.hpp"
//#include "BufferWrapper.hpp"
//#include "PythonGil.hpp"
//
//namespace firemelon
//{
//	typedef boost::shared_ptr<BufferWrapper> BufferWrapperPtr;
//
//	class SerializerCore
//	{
//	public:
//
//		SerializerCore();
//		SerializerCore(const SerializerCore &obj);
//		virtual ~SerializerCore();
//		
//		void				addInt32Py(int data);
//		void				addInt32(int data);
//		void				addFloatPy(float data);
//		void				addFloat(float data);
//		void				addBooleanPy(bool data);
//		void				addBoolean(bool data);
//		void				addStringPy(std::string data);
//		void				addString(std::string data);
//		
//		int					getInt32Py();
//		int					getInt32();
//		float				getFloatPy();
//		float				getFloat();
//		bool				getBooleanPy();
//		bool				getBoolean();
//		std::string			getStringPy();
//		std::string			getString();
//
//		void				clear();
//		
//		// Convert the contained data into a byte stream.
//		void				serialize();
//
//		// Convert the byte stream back into the original data.
//		void				deserialize();
//		
//		BufferWrapperPtr	getBufferWrapper();
//		void				setBufferWrapper(BufferWrapperPtr value);
//
//	private:
//		
//		std::vector<std::string>	strings_;
//		std::vector<int>			integers_;
//		std::vector<float>			floats_;
//		std::vector<bool>			booleans_;
//
//		NetworkUtility				networkUtility_;
//
//		BufferWrapperPtr			bufferWrapper_;
//		
//		unsigned int				stringCount_;
//		unsigned int				integerCount_;
//		unsigned int				floatCount_;
//		unsigned int				boolCount_;
//		
//		unsigned int				currentIntegerReadIndex_;
//		unsigned int				currentFloatReadIndex_;
//		unsigned int				currentBooleanReadIndex_;
//		unsigned int				currentStringReadIndex_;
//
//		bool						isSerialized_;
//	};
//}
//
//#endif // _SERIALIZERCORE_HPP_