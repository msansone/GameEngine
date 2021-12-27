#include "..\..\Headers\EngineCore\NetworkUtility.hpp"

using namespace firemelon;
using namespace boost::uuids;

NetworkUtility::NetworkUtility()
{
}

NetworkUtility::~NetworkUtility()
{
}

char NetworkUtility::getLittleEndianByteFromDouble(double data, char position)
{	
	// Return a byte from the given double as if it were in little endian format.
	// This is so that when it is transferred across the network, it can safely be
	// assumed to be little endian no matter where it came from.
	bytemap byteConverter;

	byteConverter.double_data = data;

    if (getIsLittleEndian() == true) 
	{
		// If the data is already little endian, no conversion is necessary. Simply return the byte at the request position.	
		
		return byteConverter.byte_data[position];
    } 
	else
	{
		// If the data is big endian, convert the position.
		return byteConverter.byte_data[sizeof(double) - position - 1];
	}
}

char NetworkUtility::getLittleEndianByteFromInt(int data, char position)
{
	// Return a byte from the given integer as if it were in little endian format.
	// This is so that when it is transferred across the network, it can safely be
	// assumed to be little endian no matter where it came from.
	bytemap byteConverter;

    if (getIsLittleEndian() == true) 
	{
		// If the data is already little endian, no conversion is necessary. Simply return the byte at the request position.	
		byteConverter.int_data = data;

		return byteConverter.byte_data[position];
    } 
	else
	{
		// If the data is big endian, convert the position.
		return byteConverter.byte_data[sizeof(int) - position - 1];
	}
}

char NetworkUtility::getLittleEndianByteFromFloat(float data, char position)
{
	// Return a byte from the given integer as if it were in little endian format.
	// This is so that when it is transferred across the network, it can safely be
	// assumed to be little endian no matter where it came from.
	bytemap byteConverter;

    if (getIsLittleEndian() == true) 
	{
		// If the data is already little endian, no conversion is necessary. Simply return the byte at the request position.	
		byteConverter.float_data = data;

		return byteConverter.byte_data[position];
    } 
	else
	{
		// If the data is big endian, convert the position.
		return byteConverter.byte_data[sizeof(int) - position - 1];
	}
}

uuid NetworkUtility::buildUuid(char* data, int offset)
{
	int uuidSize = boost::uuids::uuid::static_size();
				
	boost::uuids::uuid uuid;			
	
	memcpy_s(uuid.data, uuidSize, data + offset, uuidSize);

	return uuid;
}

int	NetworkUtility::buildInteger(char* data, int offset)
{
	return buildInteger(data[offset + 3], 
						data[offset + 2], 
						data[offset + 1], 
						data[offset]);
}

int	NetworkUtility::buildInteger(char byte0, char byte1, char byte2, char byte3)
{
	// The byte order passed in will be little endian, (assuming it was extracted from the corresponding method in this class)
	bytemap byteConverter;
	
    if (getIsLittleEndian() == true) 
	{
		// Little endian		
		byteConverter.byte_data[0] = byte0;
		byteConverter.byte_data[1] = byte1;
		byteConverter.byte_data[2] = byte2;
		byteConverter.byte_data[3] = byte3;
    } 
	else
	{
		// Big endian		
		byteConverter.byte_data[0] = byte3;
		byteConverter.byte_data[1] = byte2;
		byteConverter.byte_data[2] = byte1;
		byteConverter.byte_data[3] = byte0;
	}

	int ret = byteConverter.int_data;

	return ret;
}

float NetworkUtility::buildFloat(char byte0, char byte1, char byte2, char byte3)
{
	// The byte order passed in will be little endian, (assuming it was extracted from the corresponding method in this class)

	bytemap byteConverter;
	
    if (getIsLittleEndian() == true) 
	{
		// Little endian		
		byteConverter.byte_data[0] = byte0;
		byteConverter.byte_data[1] = byte1;
		byteConverter.byte_data[2] = byte2;
		byteConverter.byte_data[3] = byte3;
    } 
	else
	{
		// Big endian		
		byteConverter.byte_data[0] = byte3;
		byteConverter.byte_data[1] = byte2;
		byteConverter.byte_data[2] = byte1;
		byteConverter.byte_data[3] = byte0;
	}

	float ret = byteConverter.float_data;

	return ret;
}

double NetworkUtility::buildDouble(char byte0, char byte1, char byte2, char byte3, char byte4, char byte5, char byte6, char byte7)
{
	// The byte order passed in will be little endian, (assuming it was extracted from the corresponding method in this class)

	bytemap byteConverter;
	
    if (getIsLittleEndian() == true) 
	{
		// Little endian		
		byteConverter.byte_data[0] = byte0;
		byteConverter.byte_data[1] = byte1;
		byteConverter.byte_data[2] = byte2;
		byteConverter.byte_data[3] = byte3;
		byteConverter.byte_data[4] = byte4;
		byteConverter.byte_data[5] = byte5;
		byteConverter.byte_data[6] = byte6;
		byteConverter.byte_data[7] = byte7;
    } 
	else
	{
		// Big endian		
		byteConverter.byte_data[0] = byte7;
		byteConverter.byte_data[1] = byte6;
		byteConverter.byte_data[2] = byte5;
		byteConverter.byte_data[3] = byte4;
		byteConverter.byte_data[4] = byte3;
		byteConverter.byte_data[5] = byte2;
		byteConverter.byte_data[6] = byte1;
		byteConverter.byte_data[7] = byte0;
	}

	double ret = byteConverter.double_data;

	return ret;
}

bool NetworkUtility::getIsLittleEndian()
{
	bytemap byteConverter;

	int testPattern = 0x12345678;
	byteConverter.int_data = testPattern;

    if (byteConverter.byte_data[0] == 0x78) 
	{
		return true;
	}
	else
	{
		return false;
	}
}