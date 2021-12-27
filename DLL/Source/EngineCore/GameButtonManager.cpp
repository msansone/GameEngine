#include "..\..\Headers\EngineCore\GameButtonManager.hpp"

using namespace firemelon;

GameButtonManager::GameButtonManager()
{
}

GameButtonManager::~GameButtonManager()
{
}

std::string GameButtonManager::getGameButtonNamePy(GameButtonId buttonId)
{
	PythonReleaseGil unlocker;

	return getGameButtonName(buttonId);
}

std::string GameButtonManager::getGameButtonName(GameButtonId buttonId)
{
	return gameButtonNameIdMap_[buttonId];
}

std::string GameButtonManager::getGameButtonLabelPy(GameButtonId buttonId)
{
	PythonReleaseGil unlocker;

	return getGameButtonLabel(buttonId);
}

std::string GameButtonManager::getGameButtonLabel(GameButtonId buttonId)
{
	if (gameButtonLabelIdMap_[buttonId] == "")
	{
		return getGameButtonName(buttonId);
	}
	
	return gameButtonLabelIdMap_[buttonId];
}

std::string GameButtonManager::getGameButtonGroupNamePy(GameButtonGroupId groupId)
{
	PythonReleaseGil unlocker;

	return getGameButtonGroupName(groupId);
}

std::string GameButtonManager::getGameButtonGroupName(GameButtonGroupId groupId)
{
	return groupNameIdMap_[groupId];
}

void GameButtonManager::addButtonNameIdMapping(GameButtonId buttonId, GameButtonUuid buttonUuid, GameButtonGroupId buttonGroupId, std::string buttonName, std::string buttonLabel)
{
	// If they key hasn't been added yet, add it.
	if (gameButtonNameIdMap_.count(buttonId) == 0)
	{
		// Create the button object.
		GameButton button;
		button.name = buttonName;
		button.label = buttonLabel;
		button.id = buttonId;
		button.uuid = buttonUuid;
		button.group = buttonGroupId;

		// Map the button name and label to the ID.
		gameButtonNameIdMap_[buttonId] = buttonName;

		gameButtonLabelIdMap_[buttonId] = buttonLabel;

		// Add the button to the list.
		gameButtons_.push_back(button);

		// Add the button to the list mapped to the group.
		groupToButtonsMap_[buttonGroupId].push_back(button);
	}
}

void GameButtonManager::addButtonGroupNameIdMapping(GameButtonGroupId buttonGroupId, GameButtonGroupUuid buttonGroupUuid, std::string buttonGroupName)
{
	// If they key hasn't been added yet, add it.
	if (groupNameIdMap_.count(buttonGroupId) == 0)
	{
		// Create the button group object.
		GameButtonGroup buttonGroup;
		buttonGroup.name = buttonGroupName;
		buttonGroup.id = buttonGroupId;
		buttonGroup.uuid = buttonGroupUuid;

		// Map the button group name to the ID.
		groupNameIdMap_[buttonGroupId] = buttonGroupName;

		// Add the button group to the list.
		gameButtonGroups_.push_back(buttonGroup);
	}
}

int GameButtonManager::getGameButtonGroupCountPy()
{
	PythonReleaseGil unlocker;

	return getGameButtonGroupCount();
}

int GameButtonManager::getGameButtonGroupCount()
{
	int size = gameButtonGroups_.size();

	return size;
}

GameButtonGroupId GameButtonManager::getGameButtonGroupIdPy(int index) 
{
	PythonReleaseGil unlocker;

	return getGameButtonGroupId(index);
}

GameButtonGroupId GameButtonManager::getGameButtonGroupId(int index)
{
	int size = gameButtonGroups_.size();

	if (index >= 0 && index < size)
	{
		return gameButtonGroups_[index].id;
	}
	else
	{
		BaseIds ids;

		return ids.GAMEBUTTONGROUP_NULL;
	}	
}

GameButtonGroupId GameButtonManager::getGameButtonGroupIdForButtonPy(GameButtonId buttonId)
{
	PythonReleaseGil unlocker;

	return getGameButtonGroupIdForButton(buttonId);
}

GameButtonGroupId GameButtonManager::getGameButtonGroupIdForButton(GameButtonId buttonId)
{
	int size = gameButtons_.size();

	for (int i = 0; i < size; i++)
	{
		if (gameButtons_[i].id == buttonId)
		{
			return gameButtons_[i].group;
		}
	}

	// Bracket this off, so the BaseIds object doesn't get created unless necessary.
	{
		BaseIds ids;

		return ids.GAMEBUTTONGROUP_NULL;
	}
}

int GameButtonManager::getGameButtonCountPy()
{
	PythonReleaseGil unlocker;

	return getGameButtonCount();
}

int GameButtonManager::getGameButtonCount()
{
	int size = gameButtons_.size();

	return size;
}

int GameButtonManager::getGameButtonCountForGroupPy(GameButtonGroupId groupId)
{
	PythonReleaseGil unlocker;

	return getGameButtonCountForGroup(groupId);
}

int GameButtonManager::getGameButtonCountForGroup(GameButtonGroupId groupId)
{
	int size = groupToButtonsMap_[groupId].size();

	return size;
}

GameButtonId GameButtonManager::getGameButtonIdForGroupPy(GameButtonGroupId groupId, int index)
{
	PythonReleaseGil unlocker;

	return getGameButtonIdForGroup(groupId, index);
}

GameButtonId GameButtonManager::getGameButtonIdForGroup(GameButtonGroupId groupId, int index)
{
	int size = groupToButtonsMap_[groupId].size();

	if (index >= 0 && index < size)
	{
		return groupToButtonsMap_[groupId][index].id;
	}

	BaseIds ids;

	return ids.GAMEBUTTON_NULL;
}

GameButtonId GameButtonManager::getGameButtonIdPy(int index)
{
	PythonReleaseGil unlocker;

	return getGameButtonId(index);
}

GameButtonId GameButtonManager::getGameButtonId(int index)
{
	int size = gameButtons_.size();

	if (index >= 0 && index < size)
	{
		return gameButtons_[index].id;
	}
	else
	{
		BaseIds ids;

		return ids.GAMEBUTTON_NULL;
	}
}

GameButtonUuid GameButtonManager::getGameButtonUuid(int index)
{
	int size = gameButtons_.size();

	if (index >= 0 && index < size)
	{
		return gameButtons_[index].uuid;
	}
	else
	{
		return boost::uuids::nil_uuid();
	}
}

std::string GameButtonManager::getGameButtonUuidStringPy(int index)
{
	PythonReleaseGil unlocker;

	return getGameButtonUuidString(index);
}

std::string GameButtonManager::getGameButtonUuidString(int index)
{
	int size = gameButtons_.size();

	if (index >= 0 && index < size)
	{
		return boost::lexical_cast<std::string>(gameButtons_[index].uuid);
	}
	else
	{
		return boost::lexical_cast<std::string>(boost::uuids::nil_uuid());
	}
}