#include "..\..\Headers\EngineCore\EntityMetadataContainer.hpp"

using namespace firemelon;

EntityMetadataContainer::EntityMetadataContainer()
{
}

EntityMetadataContainer::~EntityMetadataContainer()
{
}

int EntityMetadataContainer::getEntityMetadataCountPy()
{
	PythonReleaseGil unlocker;

	return getEntityMetadataCount();
}

int EntityMetadataContainer::getEntityMetadataCount()
{
	return entityMetadataList_.size();
}

EntityMetadata EntityMetadataContainer::getEntityMetadataPy(int index)
{
	PythonReleaseGil unlocker;

	return getEntityMetadata(index);
}

EntityMetadata EntityMetadataContainer::getEntityMetadata(int index)
{
	int size = getEntityMetadataCount();

	if (index >= 0 && index < size)
	{
		return entityMetadataList_[index];
	}

	EntityMetadata emptyMetadata;

	emptyMetadata.setClassificationId(-1);
	emptyMetadata.setEntityTypeId(-1);
	emptyMetadata.setTag("");

	return emptyMetadata;
}

void EntityMetadataContainer::addEntityMetadata(EntityTypeId entityTypeId, EntityClassificationId classificationId, std::string tag)
{
	EntityMetadata newMetadata;

	newMetadata.setClassificationId(classificationId);
	newMetadata.setEntityTypeId(entityTypeId);
	newMetadata.setTag(tag);

	entityMetadataList_.push_back(newMetadata);
}
