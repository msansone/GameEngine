#include "..\..\Headers\EngineCore\TileCollisionData.hpp"

using namespace firemelon;

TileCollisionData::TileCollisionData()
{
	tileGroupId_ = 0;
}

TileCollisionData::~TileCollisionData()
{
}

void TileCollisionData::setTileGroupIdPy(unsigned int tileGroupId)
{
	PythonReleaseGil unlocker;

	setTileGroupId(tileGroupId);
}

void TileCollisionData::setTileGroupId(unsigned int tileGroupId)
{
	tileGroupId_ = tileGroupId;
}

unsigned int TileCollisionData::getTileGroupIdPy()
{
	PythonReleaseGil unlocker;

	return getTileGroupId();
}

unsigned int TileCollisionData::getTileGroupId()
{
	return tileGroupId_;
}
