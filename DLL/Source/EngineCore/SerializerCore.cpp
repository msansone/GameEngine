//#include "..\..\Headers\EngineCore\SerializerCore.hpp"
//
//using namespace firemelon;
//
//SerializerCore::SerializerCore() :
//	bufferWrapper_(new BufferWrapper),
//	networkUtility_()
//{
//	currentIntegerReadIndex_ = 0;
//	currentFloatReadIndex_ = 0;
//	currentBooleanReadIndex_ = 0;
//	currentStringReadIndex_ = 0;
//	
//	isSerialized_ = false;
//}
//
//SerializerCore::SerializerCore(const SerializerCore &obj)
//{
//	// Copy constructor. Make a copy of the shared pointer.
//	bufferWrapper_ = obj.bufferWrapper_;
//	currentFloatReadIndex_ = obj.currentFloatReadIndex_;
//	currentIntegerReadIndex_ = obj.currentIntegerReadIndex_;
//	currentStringReadIndex_ = obj.currentStringReadIndex_;
//	isSerialized_ = obj.isSerialized_;
//	stringCount_ = obj.stringCount_;
//	integerCount_ = obj.integerCount_;
//	floatCount_ = obj.floatCount_;
//	boolCount_ = obj.boolCount_;
//
//	int size = obj.strings_.size();
//	for (int i = 0; i < size; i++)
//	{
//		strings_.push_back(obj.strings_[i]);
//	}
//	
//	size = obj.integers_.size();
//	for (int i = 0; i < size; i++)
//	{
//		integers_.push_back(obj.integers_[i]);
//	}
//
//	size = obj.floats_.size();
//	for (int i = 0; i < size; i++)
//	{
//		floats_.push_back(obj.floats_[i]);
//	}
//	
//	size = obj.booleans_.size();
//	for (int i = 0; i < size; i++)
//	{
//		booleans_.push_back(obj.booleans_[i]);
//	}
//}
//
//SerializerCore::~SerializerCore()
//{
//}
//
//void SerializerCore::addInt32Py(int data)
//{
//	PythonReleaseGil unlocker;
//
//	addInt32(data);
//}
//
//void SerializerCore::addInt32(int data)
//{
//	integers_.push_back(data);
//}
//
//void SerializerCore::addFloatPy(float data)
//{
//	PythonReleaseGil unlocker;
//
//	addFloat(data);
//}
//
//void SerializerCore::addFloat(float data)
//{
//	floats_.push_back(data);
//}
//
//void SerializerCore::addBooleanPy(bool data)
//{
//	PythonReleaseGil unlocker;
//
//	addBoolean(data);
//}
//
//void SerializerCore::addBoolean(bool data)
//{
//	booleans_.push_back(data);
//}
//
//void SerializerCore::addStringPy(std::string data)
//{
//	PythonReleaseGil unlocker;
//
//	addString(data);
//}
//
//void SerializerCore::addString(std::string data)
//{
//	strings_.push_back(data);
//}
//
//int SerializerCore::getInt32Py()
//{
//	PythonReleaseGil unlocker;
//
//	return getInt32();
//}
//
//int SerializerCore::getInt32()
//{
//	return 0;
//}
//
//float SerializerCore::getFloatPy()
//{
//	PythonReleaseGil unlocker;
//
//	return getFloat();
//}
//
//float SerializerCore::getFloat()
//{
//	return 0.0;
//}
//
//bool SerializerCore::getBooleanPy()
//{
//	PythonReleaseGil unlocker;
//
//	return getBoolean();
//}
//
//bool SerializerCore::getBoolean()
//{
//	int size = booleans_.size();
//
//	if (currentBooleanReadIndex_ >= 0 && currentBooleanReadIndex_ < size)
//	{
//		return booleans_[currentBooleanReadIndex_++];
//	}
//	else
//	{
//		return false;
//	}
//}
//
//std::string SerializerCore::getStringPy()
//{
//	PythonReleaseGil unlocker;
//
//	return getString();
//}
//
//std::string SerializerCore::getString()
//{
//	int size = strings_.size();
//
//	if (currentStringReadIndex_ >= 0 && currentStringReadIndex_ < size)
//	{
//		return strings_[currentStringReadIndex_++];
//	}
//	else
//	{
//		return "";
//	}
//}
//
//void SerializerCore::clear()
//{
//	bufferWrapper_->clearBuffer();
//
//	strings_.clear();
//	integers_.clear();
//	floats_.clear();
//	booleans_.clear();
//
//	currentIntegerReadIndex_ = 0;
//	currentFloatReadIndex_ = 0;
//	currentStringReadIndex_ = 0;
//	currentBooleanReadIndex_ = 0;
//}
//
//void SerializerCore::serialize()
//{
//	// Allocate a byte array to fit the serialized data.
//	integerCount_ = integers_.size();
//	
//	floatCount_ = floats_.size();
//
//	boolCount_ = booleans_.size();
//
//	stringCount_ = strings_.size();
//
//	int stringDataLength = 0;
//
//	for (int i = 0; i < stringCount_; i++)
//	{
//		stringDataLength += strings_[i].size() + 1;
//	}
//
//	int bodySize = (integerCount_ * sizeof(int)) + (floatCount_ * sizeof(float)) + (boolCount_ * sizeof(bool)) + stringDataLength;
//
//	// Declare an array that will fit the data, plus enough space to store the counts and data length.
//	int headerSize = sizeof(int) * 5;
//
//	int dataLength = headerSize + bodySize;
//	
//
//	bufferWrapper_->allocateBuffer(dataLength);
//
//	char* data = bufferWrapper_->getBuffer();
//
//	int cursorPosition = 0;
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(dataLength, 0);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(dataLength, 1);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(dataLength, 2);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(dataLength, 3);
//	
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(integerCount_, 0);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(integerCount_, 1);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(integerCount_, 2);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(integerCount_, 3);
//	
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(floatCount_, 0);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(floatCount_, 1);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(floatCount_, 2);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(floatCount_, 3);
//	
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(boolCount_, 0);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(boolCount_, 1);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(boolCount_, 2);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(boolCount_, 3);
//	
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(stringCount_, 0);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(stringCount_, 1);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(stringCount_, 2);
//	data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(stringCount_, 3);
//	
//	for (int i = 0; i < integerCount_; i++)
//	{
//		data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(integers_[i], 0);
//		data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(integers_[i], 1);
//		data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(integers_[i], 2);
//		data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(integers_[i], 3);	
//	}
//	
//	for (int i = 0; i < floatCount_; i++)
//	{
//		data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(floats_[i], 0);
//		data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(floats_[i], 1);
//		data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(floats_[i], 2);
//		data[cursorPosition++] = networkUtility_.getLittleEndianByteFromInt(floats_[i], 3);
//	}
//	
//	for (int i = 0; i < boolCount_; i++)
//	{
//		data[cursorPosition++] = booleans_[i];
//	}
//	
//	for (int i = 0; i < stringCount_; i++)
//	{
//		const char* stringData = strings_[i].c_str();
//
//		int stringLength = strlen(stringData) + 1;
//
//		sprintf_s(data + cursorPosition, stringLength, "%s", stringData);
//		cursorPosition += stringLength;
//	}
//
//	isSerialized_ = true;
//}
//
//void SerializerCore::deserialize()
//{
//	// Allocate a byte array to fit the serialized data.
//	if (isSerialized_ == true)
//	{		
//		strings_.clear();
//		integers_.clear();
//		floats_.clear();
//		booleans_.clear();
//		
//		currentIntegerReadIndex_ = 0;
//		currentFloatReadIndex_ = 0;
//		currentStringReadIndex_ = 0;
//		currentBooleanReadIndex_ = 0;
//
//		bytemap byteConverter;
//
//		char* data = bufferWrapper_->getBuffer();
//
//		// Get the total data size.
//		//byteConverter.byte_data[0] = data[0];
//		//byteConverter.byte_data[1] = data[1];
//		//byteConverter.byte_data[2] = data[2];
//		//byteConverter.byte_data[3] = data[3];
//		
//		//dataLength_ = byteConverter.int_data;
//
//		// Get the integer count.
//		integerCount_ = networkUtility_.buildInteger(data[4], data[5], data[6], data[7]);
//
//		// Get the float count.
//		floatCount_ = networkUtility_.buildInteger(data[8], data[9], data[10], data[11]);
//		
//		// Get the bool count.
//		boolCount_ = networkUtility_.buildInteger(data[12], data[13], data[14], data[15]);
//
//		// Get the string count.
//		stringCount_ = networkUtility_.buildInteger(data[16], data[17], data[18], data[19]);
//
//		int cursorPosition = 20;
//
//		for (int i = 0; i < integerCount_; i++)
//		{
//			//byteConverter.int_data = integers_[i];
//			//memcpy(data_ + cursorPosition, byteConverter.byte_data, sizeof(int));
//			cursorPosition += sizeof(int);
//		}
//	
//		for (int i = 0; i < floatCount_; i++)
//		{
//			//byteConverter.float_data = floats_[i];
//			//memcpy(data_ + cursorPosition, byteConverter.byte_data, sizeof(float));
//			cursorPosition += sizeof(float);
//		}
//	
//		for (int i = 0; i < boolCount_; i++)
//		{
//			byteConverter.byte_data[0] = data[cursorPosition];
//			booleans_.push_back(byteConverter.bool_data);
//
//			cursorPosition += sizeof(bool);
//		}
//
//		for (int i = 0; i < stringCount_; i++)
//		{
//			std::string stringData(data + cursorPosition);
//
//			strings_.push_back(stringData);
//
//			int stringLength = stringData.size() + 1;
//
//			cursorPosition += stringLength;
//		}
//
//		// Clear the data buffer after deserialization.
//		isSerialized_ = false;
//	}
//}
//
//BufferWrapperPtr SerializerCore::getBufferWrapper()
//{
//	return bufferWrapper_;
//}
//
//
//void SerializerCore::setBufferWrapper(BufferWrapperPtr value)
//{
//	bufferWrapper_ = value;
//
//	isSerialized_ = true;
//}