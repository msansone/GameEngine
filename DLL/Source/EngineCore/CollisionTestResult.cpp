#include "..\..\Headers\EngineCore\CollisionTestResult.hpp"

using namespace firemelon;

CollisionTestResult::CollisionTestResult()
{
	collisionOccurred = false;
	
	faceNormal = boost::make_shared<Vec2<int>>(Vec2<int>(0, 0));

	intrusion = boost::make_shared<Vec2<int>>(Vec2<int>(0, 0));

}

CollisionTestResult::~CollisionTestResult()
{

}