#include "..\..\Headers\EngineCore\CollisionLogger.hpp"

using namespace firemelon;

CollisionLogger::CollisionLogger()
{
	logCollisions = false;

	types = 0;
}

CollisionLogger::~CollisionLogger()
{
}


void CollisionLogger::logCollision(CollisionLogPtr collisionLog)
{
	if (logCollisions == true)
	{
		// Default to false, and if any criteria is  met, set it to true.
		bool addToLog = true;

		// If there are entity ID parameters, check if at least one, or both, is a part of this collision.
		if (matchBothEntityIds == true)
		{
			if (validEntityInstanceIds.find(collisionLog->hitboxCollideeOwnerId) != validEntityInstanceIds.end() &&
				validEntityInstanceIds.find(collisionLog->hitboxColliderOwnerId) != validEntityInstanceIds.end())
			{
				addToLog |= true;
			}
		}
		else
		{
			if (validEntityInstanceIds.find(collisionLog->hitboxCollideeOwnerId) != validEntityInstanceIds.end() ||
				validEntityInstanceIds.find(collisionLog->hitboxColliderOwnerId) != validEntityInstanceIds.end())
			{
				addToLog |= true;
			}
		}

		// If there are hitbox ID parameters, check if at least one is a part of this collision.
		if (validHitboxIds.find(collisionLog->hitboxCollideeId) != validHitboxIds.end() ||
			validHitboxIds.find(collisionLog->hitboxColliderId) != validHitboxIds.end())
		{
			addToLog |= true;
		}

		if (addToLog == true)
		{
			loggedCollisions_.push_back(collisionLog);
		}
	}
}