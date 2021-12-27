/* -------------------------------------------------------------------------
** Types.hpp
**
** This file defines common types that are used in various modules in the engine.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _TYPES_HPP_
#define _TYPES_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <string>
#include <map>

#include <boost/python.hpp>
#include <boost/thread/mutex.hpp>
#include <boost/shared_ptr.hpp>

#include "PythonGil.hpp"

namespace firemelon
{
	typedef boost::python::object PyObj;
	
	enum EntityClassification
	{
		ENTITY_CLASSIFICATION_ACTOR = 0,
		ENTITY_CLASSIFICATION_EVENT = 1,
		ENTITY_CLASSIFICATION_HUDELEMENT = 2,
		ENTITY_CLASSIFICATION_CAMERA = 3,
		ENTITY_CLASSIFICATION_TILE = 4,
		ENTITY_CLASSIFICATION_PARTICLEEMITTER = 5,
		ENTITY_CLASSIFICATION_PARTICLE = 6
	};

	struct Rect
	{
		// Python wrappers to ensure GIL is released.
		int getXPy() { PythonReleaseGil unlocker; return x; };
		int getYPy() { PythonReleaseGil unlocker; return y; };
		int getWPy() { PythonReleaseGil unlocker; return w; };
		int getHPy() { PythonReleaseGil unlocker; return h; };
		void setXPy(int value) { PythonReleaseGil unlocker; x = value; };
		void setYPy(int value) { PythonReleaseGil unlocker; y = value; };
		void setWPy(int value) { PythonReleaseGil unlocker; w = value; };
		void setHPy(int value) { PythonReleaseGil unlocker; h = value; };

		int x;
		int y;
		int w;
		int h;
		int area;
	};

	enum StageOrigin
	{
		STAGE_ORIGIN_TOP_LEFT = 0,
		STAGE_ORIGIN_CENTER = 1,
		STAGE_ORIGIN_TOP_MIDDLE = 2,
		STAGE_ORIGIN_TOP_RIGHT = 3,
		STAGE_ORIGIN_MIDDLE_LEFT = 4,
		STAGE_ORIGIN_MIDDLE_RIGHT = 5,
		STAGE_ORIGIN_BOTTOM_LEFT = 6,
		STAGE_ORIGIN_BOTTOM_MIDDLE = 7,
		STAGE_ORIGIN_BOTTOM_RIGHT = 8,
		STAGE_ORIGIN_CUSTOM = 9
	};

	struct FIREMELONAPI Vertex2
	{
		float x;
		float y;
	};

	struct FIREMELONAPI Quad
	{
		Vertex2 vertices[4];
	};
	
	typedef std::map<std::string, std::string> stringmap;
	
	typedef boost::shared_ptr<boost::mutex> MutexPtr;
}

#endif //_TYPES_HPP_