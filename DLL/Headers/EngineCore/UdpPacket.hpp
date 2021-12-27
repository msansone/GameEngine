///* -------------------------------------------------------------------------
//** UdpPacket.hpp
//**
//** UdpPacket.hpp is used to encode and decode the various kinds of packets 
//** which gets sent to and from the server. 
//** 
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _UDPPACKET_HPP_
//#define _UDPPACKET_HPP_
//
//#include <cstdio>
//#include <cstdlib>
//#include <cstring>
//#include <iostream>
//#include <sstream>
//
//#include <SDL_net.h>
//#include <boost/signals2.hpp>
//
//namespace firemelon
//{
//	enum UdpPacketType
//	{
//		UDP_PACKET_CONNECT = 0,
//		UDP_PACKET_JOIN = 1,
//		UDP_PACKET_INPUT = 2,
//		UDP_PACKET_GET_VISIBLE_ENTITIES = 3,
//		UDP_PACKET_ENTITY_LIST = 4,
//		UDP_PACKET_LOADING = 5,
//		UDP_PACKET_SERVER_ID_LIST = 6,
//		UDP_PACKET_HEARTBEAT = 7,
//		UDP_PACKET_AWAITING_ACTIVATION = 8
//	};
//	
//	class UdpPacket
//	{
//	public:
//		friend class UdpClient;
//		friend class UdpServer;
//
//		// Assign a protocol ID to the game, so any packets that don't begin with this ID can be
//		// disregarded. Generated randomly.
//		enum { FIREMELON_PROTOCOL_ID = 0x1b34b0d7 };
//		
//		enum { HEADER_BODYSIZE_LENGTH = 3 };
//		enum { HEADER_LENGTH = 16 };
//		enum { MAX_BODY_LENGTH = 496 };
//		enum { MAX_PACKET_SIZE = 512 };
//		
//		UdpPacket();
//		UdpPacket(UdpPacketType packetType);
//		//UdpPacket(const UdpPacket &obj);
//		virtual ~UdpPacket();
//		
//		const char*		getData() const;
//  
//		char*			getData();
//
//		size_t			getLength() const;
//
//		const char*		getBody() const;
//
//		char*			getBody();
//
//		size_t			getBodyLength() const;
//
//		void			setBodyLength(size_t newLength);
//  
//		UdpPacketType	getPacketType();
//		
//		unsigned int	getSequence();
//		
//		unsigned int	getAck();
//		void			setAck(unsigned int value);
//
//		bool			decodeHeader();
//
//		// The header contains the protocol ID, followed by the packet type, followed by the body length.
//		void			encodeHeader();
//
//		std::string		receivedFromIpAddress;
//
//	private:
//		
//		std::string	getPacketIpString();
//
//		UDPpacket*		sdlPacket_;
//
//		UdpPacketType	packetType_;
//
//		char			data_[MAX_PACKET_SIZE];
//		unsigned int	bodyLength_;
//		
//		// Auto-incrementing static variable used to generate a sequence number.
//		static unsigned int	seqCounter_;
//		
//		unsigned int	sequence_;
//		unsigned int	ack_;
//	};
//}
//
//#endif // _UDPPACKET_HPP_