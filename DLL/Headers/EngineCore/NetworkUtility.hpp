/* -------------------------------------------------------------------------
** NetworkUtility.hpp
**
** The NetworkUtility class contains functions to assist with the network
** layer.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _NETWORKUTILITY_HPP_
#define _NETWORKUTILITY_HPP_

#include <boost/uuid/uuid.hpp>
#include <string>

namespace firemelon
{	
	union bytemap
	{
		unsigned char	byte_data[8];
		double			double_data;
		float			float_data;
		int				int_data;
		unsigned int	uint_data;
		bool			bool_data;
	};

	class NetworkUtility
	{
	public:

		NetworkUtility();
		virtual ~NetworkUtility();
		
		char				getLittleEndianByteFromInt(int data, char position);
		char				getLittleEndianByteFromFloat(float data, char position);
		char				getLittleEndianByteFromDouble(double data, char position);
		
		boost::uuids::uuid	buildUuid(char* data, int offset);
		int					buildInteger(char* data, int offset);
		int					buildInteger(char byte0, char byte1, char byte2, char byte3);
		float				buildFloat(char byte0, char byte1, char byte2, char byte3);
		double				buildDouble(char byte0, char byte1, char byte2, char byte3, char byte4, char byte5, char byte6, char byte7);

	private:

		bool	getIsLittleEndian();
	};
}

#endif // _NETWORKUTILITY_HPP_