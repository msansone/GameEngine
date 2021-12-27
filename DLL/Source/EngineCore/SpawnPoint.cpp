#include "..\..\Headers\EngineCore\SpawnPoint.hpp"

using namespace firemelon;

SpawnPoint::SpawnPoint()
{
	BaseIds ids;

	id_ = ids.SPAWNPOINT_NULL;

	x_ = 0;
	y_ = 0;
}

SpawnPoint::~SpawnPoint()
{
}

void SpawnPoint::setId(SpawnPointId id)
{
	id_ = id;
}

SpawnPointId SpawnPoint::getId()
{
	return id_;
}

void SpawnPoint::setX(int x)
{
	x_ = x;
}

int	SpawnPoint::getX()
{
	return x_;
}

void SpawnPoint::setY(int y)
{
	y_ = y;
}

int	SpawnPoint::getY()
{
	return y_;
}

void SpawnPoint::setLayer(int layer)
{
	layer_ = layer;
}

int	SpawnPoint::getLayer()
{
	return layer_;
}