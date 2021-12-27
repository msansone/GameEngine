#include "..\..\Headers\EngineCore\RoomManager.hpp"

#define BOOST_ALL_NO_LIB

using namespace firemelon;
using namespace boost::python;

//BOOST_PYTHON_MEMBER_FUNCTION_OVERLOADS(statemachinecontroller_overloads, StateMachineController::setStateByNamePy, 1, 2) KEEP THIS! I need it as an example for how to do overloads.

// Define all of the firemelon classes to be exposed to Python here.
BOOST_PYTHON_MODULE(firemelon) {

	enum_<AlphaGradientDirection>("AlphaGradientDirection")
		.value("NONE", ALPHA_GRADIENT_NONE)
		.value("NORTH", ALPHA_GRADIENT_NORTH)
		.value("SOUTH", ALPHA_GRADIENT_SOUTH)
		.value("EAST", ALPHA_GRADIENT_EAST)
		.value("WEST", ALPHA_GRADIENT_WEST)
		.value("NORTHEAST", ALPHA_GRADIENT_NORTHEAST)
		.value("NORTHWEST", ALPHA_GRADIENT_NORTHWEST)
		.value("SOUTHEAST", ALPHA_GRADIENT_SOUTHEAST)
		.value("SOUTHWEST", ALPHA_GRADIENT_SOUTHWEST)
		.value("RADIAL", ALPHA_GRADIENT_RADIAL);

	enum_<AnimationSlotOrigin>("AnimationSlotOrigin")
		.value("TOP_LEFT", ANIMATION_SLOT_ORIGIN_TOP_LEFT)
		.value("CENTER", ANIMATION_SLOT_ORIGIN_CENTER)
		.value("TOP_MIDDLE", ANIMATION_SLOT_ORIGIN_TOP_MIDDLE);

	enum_<AnimationStyle>("AnimationStyle")
        .value("REPEAT", ANIMATION_STYLE_REPEAT)
        .value("SINGLE", ANIMATION_STYLE_SINGLE)
        .value("SINGLE_PERSIST", ANIMATION_STYLE_SINGLE_PERSIST)
        .value("SINGLE_END_STATE", ANIMATION_STYLE_SINGLE_END_STATE);

	enum_<Axis>("Axis")
		.value("X", AXIS_X)
		.value("Y", AXIS_Y)
		.value("XY", AXIS_XY);

	// ButtonState enum Python definition.
	enum_<ButtonState>("ButtonState")
        .value("DOWN", BUTTON_STATE_DOWN)
        .value("UP", BUTTON_STATE_UP);

	// CollisionResolution enum Python definition.
	enum_<CollisionResolution>("CollisionResolution")
		.value("SOLID_PRIORITY", COLLISION_RESOLUTION_SOLID_PRIORITY)
		.value("SOLID_YIELD", COLLISION_RESOLUTION_SOLID_YIELD)
		.value("PERMEABLE", COLLISION_RESOLUTION_PERMEABLE);
	
	enum_<CollisionStyle>("CollisionStyle")
		.value("SOLID", COLLISION_STYLE_SOLID)
		.value("INCLINE", COLLISION_STYLE_INCLINE)
		.value("DECLINE", COLLISION_STYLE_DECLINE)
		.value("INVERTED_INCLINE", COLLISION_STYLE_INVERTED_INCLINE)
		.value("INVERTED_DECLINE", COLLISION_STYLE_INVERTED_DECLINE)
		.value("ONE_WAY_TOP", COLLISION_STYLE_ONE_WAY_TOP)
		.value("ONE_WAY_BOTTOM", COLLISION_STYLE_ONE_WAY_BOTTOM)
		.value("ONE_WAY_LEFT", COLLISION_STYLE_ONE_WAY_LEFT)
		.value("ONE_WAY_RIGHT", COLLISION_STYLE_ONE_WAY_RIGHT);

	enum_<MessagePriority>("MessagePriority")
        .value("IMMEDIATE", MESSAGE_PRIORITY_IMMEDIATE)
        .value("NORMAL", MESSAGE_PRIORITY_NORMAL);

	enum_<PanelElementType>("PanelElementType")
		.value("PANEL", PANEL_ELEMENT_PANEL)
		.value("WIDGET", PANEL_ELEMENT_WIDGET);

	enum_<PanelControlFlow>("PanelControlFlow")
		.value("BOOK", PANEL_CONTROL_FLOW_BOOK)
		.value("COLLAGE", PANEL_CONTROL_FLOW_COLLAGE);

	enum_<PanelHorizontalAlignment>("PanelHorizontalAlignment")
		.value("LEFT", PANEL_HORIZONTAL_ALIGNMENT_LEFT)
		.value("CENTER", PANEL_HORIZONTAL_ALIGNMENT_CENTER)
		.value("RIGHT", PANEL_HORIZONTAL_ALIGNMENT_RIGHT);

	enum_<PanelVerticalAlignment>("PanelVerticalAlignment")
		.value("BOTTOM", PANEL_VERTICAL_ALIGNMENT_BOTTOM)
		.value("CENTER", PANEL_VERTICAL_ALIGNMENT_CENTER)
		.value("TOP", PANEL_VERTICAL_ALIGNMENT_TOP);

	enum_<StageOrigin>("StageOrigin")
		.value("TOP_LEFT", STAGE_ORIGIN_TOP_LEFT)
		.value("CENTER", STAGE_ORIGIN_CENTER)
		.value("TOP_MIDDLE", STAGE_ORIGIN_TOP_MIDDLE);

	// AudioSource Python Definition
	class_<AudioSource, boost::shared_ptr<AudioSource>>("AudioSource", init<std::string, int, int>())
		.add_property("isPlaying", &AudioSource::getIsPlayingPy)
		.add_property("name", &AudioSource::getNamePy)
		.add_property("position", &AudioSource::getPositionPy)
		.def("play", &AudioSource::playPy)
		.def("stop", &AudioSource::stopPy);

	// AudioSourceProperties Python Definition
	class_<AudioSourceProperties, boost::shared_ptr<AudioSourceProperties>>("AudioSourceProperties")
		.add_property("x", &AudioSourceProperties::getXPy, &AudioSourceProperties::setXPy)
		.add_property("y", &AudioSourceProperties::getYPy, &AudioSourceProperties::setYPy)
		.add_property("layer", &AudioSourceProperties::getLayerPy, &AudioSourceProperties::setLayerPy)
		.add_property("name", &AudioSourceProperties::getNamePy, &AudioSourceProperties::setNamePy)
		.add_property("audioId", &AudioSourceProperties::getAudioIdPy, &AudioSourceProperties::setAudioIdPy)
		.add_property("autoplay", &AudioSourceProperties::getAutoplayPy, &AudioSourceProperties::setAutoplayPy)
		.add_property("loop", &AudioSourceProperties::getLoopPy, &AudioSourceProperties::setLoopPy)
		.add_property("minDistance", &AudioSourceProperties::getMinDistancePy, &AudioSourceProperties::setMinDistancePy)
		.add_property("maxDistance", &AudioSourceProperties::getMaxDistancePy, &AudioSourceProperties::setMaxDistancePy)
		.add_property("volume", &AudioSourceProperties::getVolumePy, &AudioSourceProperties::setVolumePy);

	// EngineController Python Definition
	class_<EngineController>("EngineController")
		.def("advanceSimulationFrame", &EngineController::advanceSimulationFrame)
		.def("exit", &EngineController::exitPy)
		.def("runCommand", &EngineController::runCommandPy)
		.def("getIdFromName", &EngineController::getIdFromNamePy)
		.def("getNameFromId", &EngineController::getNameFromIdPy)
		.def("saveScreenshot", &EngineController::saveScreenshotPy)
		.def("startSimulation", &EngineController::startSimulationPy)
		.def("stopSimulation", &EngineController::stopSimulationPy)
		.add_property("simulationTimeScale", &EngineController::getSimulationTimeScalePy, &EngineController::setSimulationTimeScalePy);
		
	// EngineConfig Python Definition
	class_<EngineConfig>("EngineConfig")
		.add_property("fpsLimiter", &EngineConfig::getFpsLimiterPy, &EngineConfig::setFpsLimiterPy)
		.add_property("interpolateFrames", &EngineConfig::getInterpolateFramesPy, &EngineConfig::setInterpolateFramesPy);
	
	// RoomManager Python Definition
	class_<RoomManager>("RoomManager", no_init)
		.add_property("showingRoomId", &RoomManager::getShownRoomIdPy)
		.def("loadRoom", &RoomManager::loadRoomPy)
		.def("loadRoomByName", &RoomManager::loadRoomByNamePy)
		.def("showRoom", &RoomManager::showRoomPy)
		.def("showRoomByName", &RoomManager::showRoomByNamePy)
		.def("unloadRoom", &RoomManager::unloadRoomPy);
	
	// RoomContainer Python Definition
	class_<RoomContainer>("RoomContainer")
		.def("getRoom", &RoomContainer::getRoomPy)
		.def("getShownRoom", &RoomContainer::getShownRoomPy);
		//.def("getRoom", &RoomContainer::getRoomPy, return_internal_reference<>())
		//.def("getShownRoom", &RoomContainer::getShownRoomPy, return_internal_reference<>());

	// ShowRoomParameters Python Definition
	class_<ShowRoomParameters>("ShowRoomParameters")
		.add_property("roomId", &ShowRoomParameters::roomId, &ShowRoomParameters::roomId)
		.add_property("transitionId", &ShowRoomParameters::transitionId, &ShowRoomParameters::transitionId)
		.add_property("transitionTime", &ShowRoomParameters::transitionTime, &ShowRoomParameters::transitionTime);

	// TransitionManager Python Definition
	class_<TransitionManager>("TransitionManager")
		.add_property("isTransitionActive", &TransitionManager::getIsTransitioningPy)
		.add_property("isAsyncTransitionActive", &TransitionManager::getIsTransitioningAsyncPy)
		.def("activateTransitionAsync", &TransitionManager::activateTransitionAsyncPy)
		.def("endTransitionAsync", &TransitionManager::endTransitionAsyncPy)
		.def("activateTransition", &TransitionManager::activateTransitionPy);

	object (Room::*getEntityPyInstanceById)(int) = &Room::getEntityPyInstanceById;
	object (Room::*getEntityPyInstanceByIdWithType)(EntityTypeId, int) = &Room::getEntityPyInstanceById;
	object (Room::*getEntityPyInstanceByName)(std::string) = &Room::getEntityPyInstanceByName;
	object (Room::*getEntityPyInstanceByNameWithType)(EntityTypeId, std::string) = &Room::getEntityPyInstanceByName;

	// Room Python Definition
	class_<Room, boost::shared_ptr<Room>, boost::noncopyable>("Room")
		.def("createAudioSource", &Room::createAudioSourcePy)
		.def("createCamera", &Room::createCameraPy)
		.def("createEntity", &Room::createEntityPy)
		.def("getEntityById", getEntityPyInstanceById)
		.def("getEntityById", getEntityPyInstanceByIdWithType)
		.def("getEntityByName", getEntityPyInstanceByName)
		.def("getEntityByName", getEntityPyInstanceByNameWithType)
		.def("createParticleEmitter", &Room::createParticleEmitterPy)
		.add_property("entityTypeLists", &Room::getEntityTypeMapPy)
		.add_property("metadata", &Room::getMetadataPy);

	// PythonInstanceWrapper Python Definition
	class_<PythonInstanceWrapper>("PythonInstanceWrapper")
		.add_property("pythonObject", &PythonInstanceWrapper::getPyInstance);

	// EntityController Python Definition
	class_<EntityController, boost::shared_ptr<EntityController>>("EntityController")
		.def("changeName", &EntityController::changeNamePy)
		.def("changeRoom", &EntityController::changeRoomPy)
		.def("remove", &EntityController::removePy)
		.def("attachTo", &EntityController::attachToPy)
		.def("detach", &EntityController::detachPy)
		.def("getAttachedTo", &EntityController::getAttachedToPy)
		.def("getAttachedEntityCount", &EntityController::getAttachedEntityCountPy)
		.def("getAttachedEntity", &EntityController::getAttachedEntityPy)
		.def("setAttachmentAxis", &EntityController::setAttachmentAxisPy)
		.def("attachAudioSource", &EntityController::attachAudioSourcePy);
		// These are from before switching from raw pointers to shared_ptr. Keep them for now until I know I don't need it anymore.
		//.def("getAttachedTo", make_function(&EntityController::getAttachedToPy, return_internal_reference<>()));
		//.def("getAttachedEntity", make_function(&EntityController::getAttachedEntityPy, return_internal_reference<>()));

	// CameraController Python Definition
	class_<CameraController, boost::shared_ptr<CameraController>, bases<EntityController>>("CameraController")
		.def("changeRoom", &CameraController::changeRoomPy)
		.def("remove", &CameraController::removePy)
		.def("attachTo", &CameraController::attachToEntityPy)
		.def("detach", &CameraController::detachPy)
		.def("getAttachedTo", &CameraController::getAttachedToPy)
		.def("getAttachedEntityCount", &CameraController::getAttachedEntityCountPy)
		.def("getAttachedEntity", &CameraController::getAttachedEntityPy);
		//.def("getAttachedTo", make_function(&CameraController::getAttachedToPy, return_internal_reference<>()))
		//.def("getAttachedEntity", make_function(&CameraController::getAttachedEntityPy, return_internal_reference<>()));

	// CameraManager Python Definition
	class_<CameraManager, boost::shared_ptr<CameraManager>>("CameraManager")
		.add_property("activeCamera", &CameraManager::getActiveCameraPy, &CameraManager::setActiveCameraPy);

	// EntityCreationParameters Python Definition
	class_<EntityCreationParameters>("EntityCreationParameters")
		.add_property("x", &EntityCreationParameters::getXPy, &EntityCreationParameters::setXPy)
		.add_property("y", &EntityCreationParameters::getYPy, &EntityCreationParameters::setYPy)
		.add_property("w", &EntityCreationParameters::getWPy, &EntityCreationParameters::setWPy)
		.add_property("h", &EntityCreationParameters::getHPy, &EntityCreationParameters::setHPy)
		.add_property("layer", &EntityCreationParameters::getLayerPy, &EntityCreationParameters::setLayerPy)
		.add_property("spawnPointId", &EntityCreationParameters::getSpawnPointIdPy, &EntityCreationParameters::setSpawnPointIdPy)
		.add_property("renderOrder", &EntityCreationParameters::getRenderOrderPy, &EntityCreationParameters::setRenderOrderPy)
		.add_property("acceptInput", &EntityCreationParameters::getAcceptInputPy, &EntityCreationParameters::setAcceptInputPy)
		.add_property("inputChannel", &EntityCreationParameters::getInputChannelPy, &EntityCreationParameters::setInputChannelPy)
		.add_property("attachCamera", &EntityCreationParameters::getAttachCameraPy, &EntityCreationParameters::setAttachCameraPy)
		.add_property("initialStateName", &EntityCreationParameters::getInitialStateNamePy, &EntityCreationParameters::setInitialStateNamePy)
		.add_property("entityName", &EntityCreationParameters::getEntityNamePy, &EntityCreationParameters::setEntityNamePy)
		.add_property("entityTypeId", &EntityCreationParameters::getEntityTypeIdPy, &EntityCreationParameters::setEntityTypeIdPy)
		.def("addProperty", &EntityCreationParameters::addPropertyPy);
		
	class_<Vec2<float>, boost::shared_ptr<Vec2<float>>>("Vec2d", init<float, float>())
		.add_property("x", &Vec2<float>::getXPy, &Vec2<float>::setXPy)
		.add_property("y", &Vec2<float>::getYPy, &Vec2<float>::setYPy);

	class_<Vec2<int>, boost::shared_ptr<Vec2<int>>>("Vec2i", init<int, int>())
		.add_property("x", &Vec2<int>::getXPy, &Vec2<int>::setXPy)
		.add_property("y", &Vec2<int>::getYPy, &Vec2<int>::setYPy);
	
	class_<PhysicsConfig, boost::shared_ptr<PhysicsConfig>>("Physics")
		.add_property("gravity", &PhysicsConfig::getGravityPy)
		.add_property("linearDamp", &PhysicsConfig::getLinearDampPy)
		.add_property("minimumVelocity", &PhysicsConfig::getMinimumVelocityPy)
		.add_property("timescale", &PhysicsConfig::getTimeScalePy, &PhysicsConfig::setTimeScalePy);

	class_<PhysicsSnapshot>("PhysicsSnapshot")
		.add_property("position", &PhysicsSnapshot::getPositionPy)
		.add_property("velocity", &PhysicsSnapshot::getVelocityPy)
		.add_property("acceleration", &PhysicsSnapshot::getAccelerationPy)
		.add_property("movement", &PhysicsSnapshot::getMovementPy)
		.add_property("positionInt", &PhysicsSnapshot::getPositionIntPy)
		.add_property("positionIntDelta", &PhysicsSnapshot::getPositionIntDeltaPy)
		.add_property("netForce", &PhysicsSnapshot::getNetForcePy);		
		//.add_property("position", make_function(&PhysicsSnapshot::getPositionPy, return_internal_reference<>()))
		//.add_property("velocity", make_function(&PhysicsSnapshot::getVelocityPy, return_internal_reference<>()))
		//.add_property("acceleration", make_function(&PhysicsSnapshot::getAccelerationPy, return_internal_reference<>()))
		//.add_property("movement", make_function(&PhysicsSnapshot::getMovementPy, return_internal_reference<>()))
		//.add_property("positionInt", make_function(&PhysicsSnapshot::getPositionIntPy, return_internal_reference<>()))
		//.add_property("positionIntDelta", make_function(&PhysicsSnapshot::getPositionIntDeltaPy, return_internal_reference<>()))
		//.add_property("netForce", make_function(&PhysicsSnapshot::getNetForcePy, return_internal_reference<>()));

	// DynamicsController Python Definition
	class_<DynamicsController>("DynamicsController")
		.add_property("ownerId", &DynamicsController::getOwnerIdPy)
		.add_property("physicsSnapshot", make_function(&DynamicsController::getPhysicsSnapshotPy, return_internal_reference<>()))
		.add_property("physicsSnapshotPrevious", make_function(&DynamicsController::getPhysicsSnapshotPreviousPy, return_internal_reference<>()))
		.add_property("look", &DynamicsController::getLookPy)
		.add_property("ownerStageHeight", &DynamicsController::getOwnerStageHeightPy, &DynamicsController::setOwnerStageHeightPy)
		.add_property("positionX", &DynamicsController::getPositionXPy, &DynamicsController::relocatePositionXPy)
		.add_property("positionY", &DynamicsController::getPositionYPy, &DynamicsController::relocatePositionYPy)
		.add_property("x", &DynamicsController::getPositionXPy, &DynamicsController::setPositionXPy)
		.add_property("y", &DynamicsController::getPositionXPy, &DynamicsController::setPositionYPy)
		.add_property("velocityY", &DynamicsController::getPositionYPy, &DynamicsController::setVelocityYPy)
		.add_property("movementX", &DynamicsController::getMovementXPy, &DynamicsController::setMovementXPy)
		.add_property("movementY", &DynamicsController::getMovementYPy, &DynamicsController::setMovementYPy)
		.add_property("ownerStageWidth", &DynamicsController::getOwnerStageWidthPy, &DynamicsController::setOwnerStageWidthPy)
		.add_property("isAffectedByGravity", &DynamicsController::getIsAffectedByGravityPy, &DynamicsController::setIsAffectedByGravityPy)
		.add_property("physicsSettings", make_function(&DynamicsController::getLocalPhysicsConfigPy, return_internal_reference<>()))
		.add_property("useLocalPhysicsSettings", &DynamicsController::getUseLocalPhysicsSettingsPy, &DynamicsController::setUseLocalPhysicsSettingsPy)
		.def("applyForce", &DynamicsController::applyForcePy)
		.def("applyImpulse", &DynamicsController::applyImpulsePy)
		.def("clearForcesX", &DynamicsController::clearForcesXPy)
		.def("clearForcesY", &DynamicsController::clearForcesYPy)
		.def("getRenderer", &DynamicsController::getRendererPy, return_internal_reference<>())
		.def("getThis", make_function(&DynamicsController::getThisPy, return_internal_reference<>()))
		.def("initialize", &DynamicsController::initializePy)
		.def("render", &DynamicsController::renderPy)
		.def("reset", &DynamicsController::resetPy);
		//.add_property("look", make_function(&DynamicsController::getLookPy, return_internal_reference<>()))

	// StateMachineController Python Definition
	class_<StateMachineController, boost::shared_ptr<StateMachineController>, boost::noncopyable>("StateMachineController")
		.add_property("ownerId", &StateMachineController::getOwnerIdPy)
		.add_property("currentStateIndex", &StateMachineController::getCurrentStateIndexPy)
		.add_property("stateCount", &StateMachineController::getStateCountPy)
		.def("setStateByIndex", &StateMachineController::setStateByIndexPy)
		//.def("setStateByName", &StateMachineController::setStateByNamePy, statemachinecontroller_overloads()) KEEP THIS! I need it as an example for how to do overloads.
		.def("setStateByName", &StateMachineController::setStateByNamePy)
		.def("addState", &StateMachineController::addStatePy)
		.def("getState", &StateMachineController::getStatePy)
		.def("getCurrentState", &StateMachineController::getCurrentStatePy)
		.def("getStateByName", &StateMachineController::getStateByNamePy)
		.def("getStateByIndex", &StateMachineController::getStatePy)
		.def("getStateIndexFromName", &StateMachineController::getStateIndexFromNamePy)
		.def("getStateNameFromIndex", &StateMachineController::getStateNameFromIndexPy);

	// StateMachineState Python Definition
	class_<StateMachineState, boost::shared_ptr<StateMachineState>>("StateMachineState", init<std::string>());

	// Function overloads
	void (StageController::*assignAnimationByNameToSlotByNameByStateIndexPy)(int, std::string, std::string) = &StageController::assignAnimationByNameToSlotByNamePy;
	void (StageController::*assignAnimationByNameToSlotByNameByStateIndexWithSynchPy)(int, std::string, std::string, std::string) = &StageController::assignAnimationByNameToSlotByNamePy;

	void (StageController::*assignAnimationByNameToSlotByIndexByStateIndexPy)(int, int, std::string) = &StageController::assignAnimationByNameToSlotByIndexPy;
	void (StageController::*assignAnimationByNameToSlotByIndexByStateIndexWithSynchPy)(int, int, std::string, std::string) = &StageController::assignAnimationByNameToSlotByIndexPy;

	void (StageController::*assignAnimationByIdToSlotByNameByStateIndexPy)(int, std::string, int) = &StageController::assignAnimationByIdToSlotByNamePy;
	void (StageController::*assignAnimationByIdToSlotByNameByStateIndexWithSynchPy)(int, std::string, int, std::string) = &StageController::assignAnimationByIdToSlotByNamePy;

	void (StageController::*assignAnimationByIdToSlotByIndexByStateIndexPy)(int, int, int) = &StageController::assignAnimationByIdToSlotByIndexPy;
	void (StageController::*assignAnimationByIdToSlotByIndexByStateIndexWithSynchPy)(int, int, int, std::string) = &StageController::assignAnimationByIdToSlotByIndexPy;


	void (StageController::*assignAnimationByNameToSlotByNameByStateNamePy)(std::string, std::string, std::string) = &StageController::assignAnimationByNameToSlotByNamePy;
	void (StageController::*assignAnimationByNameToSlotByNameByStateNameWithSynchPy)(std::string, std::string, std::string, std::string) = &StageController::assignAnimationByNameToSlotByNamePy;

	void (StageController::*assignAnimationByNameToSlotByIndexByStateNamePy)(std::string, int, std::string) = &StageController::assignAnimationByNameToSlotByIndexPy;
	void (StageController::*assignAnimationByNameToSlotByIndexByStateNameWithSynchPy)(std::string, int, std::string, std::string) = &StageController::assignAnimationByNameToSlotByIndexPy;

	void (StageController::*assignAnimationByIdToSlotByNameByStateNamePy)(std::string, std::string, int) = &StageController::assignAnimationByIdToSlotByNamePy;
	void (StageController::*assignAnimationByIdToSlotByNameByStateNameWithSynchPy)(std::string, std::string, int, std::string) = &StageController::assignAnimationByIdToSlotByNamePy;

	void (StageController::*assignAnimationByIdToSlotByIndexByStateNamePy)(std::string, int, int) = &StageController::assignAnimationByIdToSlotByIndexPy;
	void (StageController::*assignAnimationByIdToSlotByIndexByStateNameWithSynchPy)(std::string, int, int, std::string) = &StageController::assignAnimationByIdToSlotByIndexPy;

	// StageController Python Definition
	class_<StageController, boost::shared_ptr<StageController>, boost::noncopyable>("StageController")
		.add_property("currentStageElementsIndex", &StageController::getCurrentStageElementsIndexPy)
		.add_property("extentLeft", &StageController::getExtentLeftPy, &StageController::setExtentLeftPy)
		.add_property("extentTop", &StageController::getExtentTopPy, &StageController::setExtentTopPy)
		.add_property("extentRight", &StageController::getExtentRightPy, &StageController::setExtentRightPy)
		.add_property("extentBottom", &StageController::getExtentBottomPy, &StageController::setExtentBottomPy)
		.add_property("height", &StageController::getHeightPy)
		.add_property("isVisible", &StageController::getIsVisiblePy, &StageController::setIsVisiblePy)
		.add_property("interpolateExtents", &StageController::getInterpolateExtentsPy, &StageController::setInterpolateExtentsPy)
		.add_property("interpolateRotation", &StageController::getInterpolateRotationPy, &StageController::setInterpolateRotationPy)
		.add_property("mirrorHorizontally", &StageController::getMirrorHorizontallyPy, &StageController::setMirrorHorizontallyPy)
		.add_property("hueColor", &StageController::getHueColorPy)
		.add_property("outlineColor", &StageController::getOutlineColorPy)
		.add_property("pivotPoint", &StageController::getPivotPointPy)
		.add_property("renderOrder", &StageController::getRenderOrderPy)
		.add_property("rotation", &StageController::getRotationAnglePy, &StageController::setRotationAnglePy)
		.add_property("width", &StageController::getWidthPy)
		.def("addAnimationSlotByIndex", &StageController::addAnimationSlotToStageElementsByIndexPy)
		.def("addAnimationSlotByName", &StageController::addAnimationSlotToStageElementsByNamePy)
		.def("assignAnimationToSlotByIndex", assignAnimationByNameToSlotByNameByStateIndexPy)
		.def("assignAnimationToSlotByIndex", assignAnimationByNameToSlotByNameByStateIndexWithSynchPy)
		.def("assignAnimationToSlotByIndex", assignAnimationByNameToSlotByIndexByStateIndexPy)
		.def("assignAnimationToSlotByIndex", assignAnimationByNameToSlotByIndexByStateIndexWithSynchPy)
		.def("assignAnimationToSlotByIndex", assignAnimationByIdToSlotByNameByStateIndexPy)
		.def("assignAnimationToSlotByIndex", assignAnimationByIdToSlotByNameByStateIndexWithSynchPy)
		.def("assignAnimationToSlotByIndex", assignAnimationByIdToSlotByIndexByStateIndexPy)
		.def("assignAnimationToSlotByIndex", assignAnimationByIdToSlotByIndexByStateIndexWithSynchPy)
		.def("assignAnimationToSlotByName", assignAnimationByNameToSlotByNameByStateNamePy)
		.def("assignAnimationToSlotByName", assignAnimationByNameToSlotByNameByStateNameWithSynchPy)
		.def("assignAnimationToSlotByName", assignAnimationByNameToSlotByIndexByStateNamePy)
		.def("assignAnimationToSlotByName", assignAnimationByNameToSlotByIndexByStateNameWithSynchPy)
		.def("assignAnimationToSlotByName", assignAnimationByIdToSlotByNameByStateNamePy)
		.def("assignAnimationToSlotByName", assignAnimationByIdToSlotByNameByStateNameWithSynchPy)
		.def("assignAnimationToSlotByName", assignAnimationByIdToSlotByIndexByStateNamePy)
		.def("assignAnimationToSlotByName", assignAnimationByIdToSlotByIndexByStateNameWithSynchPy)
		.def("getCurrentStageElements", &StageController::getCurrentStageElementsPy)
		.def("getStageElementsByName", &StageController::getStageElementsByNamePy)
		.def("getStageElementsByIndex", &StageController::getStageElementsPy)
		.def("synchAnimationSlots", &StageController::synchAnimationSlotsPy);

	
	class_<StageElements, boost::shared_ptr<StageElements>>("StageElements", init<std::string>())
		.def("addHitboxReference", &StageElements::addHitboxReferencePy)
		.def("removeHitboxReferenceByIndex", &StageElements::removeHitboxReferenceByIndexPy)
		.def("removeAnimationSlot", &StageElements::removeAnimationSlotByNamePy)
		.def("getHitboxReference", &StageElements::getHitboxReferencePy)
		.def("getAnchorPoint", &StageElements::getAnchorPointByNameFromSlotByNamePy)
		.def("getAnimationId", &StageElements::getAnimationIdByNamePy)
		.def("getAnimationIdByIndex", &StageElements::getAnimationIdByIndexPy)
		.def("getAnimationPlayer", &StageElements::getAnimationPlayerByNamePy)
		.def("getAnimationPlayerByIndex", &StageElements::getAnimationPlayerByIndexPy)
		.def("getAnimationSlotBlendColor", &StageElements::getAnimationSlotBlendColorByNamePy)
		.def("getAnimationSlotBlendColorByIndex", &StageElements::getAnimationSlotBlendColorByIndexPy)
		.def("getAnimationSlotBlendPercent", &StageElements::getAnimationSlotBlendPercentByNamePy)
		.def("getAnimationSlotBlendPercentByIndex", &StageElements::getAnimationSlotBlendPercentByIndexPy)
		.def("getAnimationSlotHueColor", &StageElements::getAnimationSlotHueColorByNamePy)
		.def("getAnimationSlotHueColorByIndex", &StageElements::getAnimationSlotHueColorByIndexPy)
		.def("getAnimationSlotIndex", &StageElements::getAnimationSlotIndexPy)
		.def("getAnimationSlotName", &StageElements::getAnimationSlotNamePy)
		.def("getAnimationSlotPositionX", &StageElements::getAnimationSlotPositionXByNamePy)
		.def("getAnimationSlotPositionY", &StageElements::getAnimationSlotPositionYByNamePy)
		.def("getAnimationSlotPositionXByIndex", &StageElements::getAnimationSlotPositionXByIndexPy)
		.def("getAnimationSlotPositionYByIndex", &StageElements::getAnimationSlotPositionYByIndexPy)
		.def("setAnimationSlotPosition", &StageElements::setAnimationSlotPositionByNamePy)
		.def("getAnimationSlotRotation", &StageElements::getAnimationSlotRotationByNamePy)
		.def("setAnimationSlotBlendColor", &StageElements::setAnimationSlotBlendColorByNamePy)
		.def("setAnimationSlotBlendColorByIndex", &StageElements::setAnimationSlotBlendColorByIndexPy)
		.def("setAnimationSlotBlendPercent", &StageElements::setAnimationSlotBlendPercentByNamePy)
		.def("setAnimationSlotBlendPercentByIndex", &StageElements::setAnimationSlotBlendPercentByIndexPy)
		.def("setAnimationSlotFramesPerSecond", &StageElements::setAnimationSlotFramesPerSecondByNamePy)
		.def("setAnimationSlotHueColor", &StageElements::setAnimationSlotHueColorByNamePy)
		.def("setAnimationSlotHueColorByIndex", &StageElements::setAnimationSlotHueColorByIndexPy)
		.def("setAnimationSlotRotation", &StageElements::setAnimationSlotRotationByNamePy)
		.def("setAnimationSlotAlphaGradientRadius", &StageElements::setAnimationSlotAlphaGradientRadiusByNamePy)
		.def("setAnimationSlotAlphaGradientDirection", &StageElements::setAnimationSlotAlphaGradientDirectionByNamePy)
		.def("setAnimationSlotAlphaGradientFrom", &StageElements::setAnimationSlotAlphaGradientFromByNamePy)
		.def("setAnimationSlotAlphaGradientTo", &StageElements::setAnimationSlotAlphaGradientToByNamePy)
		.def("setAnimationSlotExtentLeft", &StageElements::setAnimationSlotExtentLeftByNamePy)
		.def("setAnimationSlotExtentTop", &StageElements::setAnimationSlotExtentTopByNamePy)
		.def("setAnimationSlotExtentRight", &StageElements::setAnimationSlotExtentRightByNamePy)
		.def("setAnimationSlotExtentBottom", &StageElements::setAnimationSlotExtentBottomByNamePy)
		.def("setAnimationSlotPosition", &StageElements::setAnimationSlotPositionByNamePy)
		.def("setAnimationSlotPositionX", &StageElements::setAnimationSlotPositionXByNamePy)
		.def("setAnimationSlotPositionY", &StageElements::setAnimationSlotPositionYByNamePy)
		.def("setAnimationSlotPositionXByIndex", &StageElements::setAnimationSlotPositionXByIndexPy)
		.def("setAnimationSlotPositionYByIndex", &StageElements::setAnimationSlotPositionYByIndexPy)
		.add_property("animationSlotCount", &StageElements::getAnimationSlotCountPy)
		.add_property("hitboxCount", &StageElements::getHitboxReferenceCountPy)
		.add_property("hueColor", &StageElements::getHueColorPy)
		.add_property("name", &StageElements::getAssociatedStateNamePy)
		.add_property("pivotPoint", &StageElements::getPivotPointPy)
		.add_property("rotation", &StageElements::getRotationAnglePy, &StageElements::setRotationAnglePy);


	// ParticleEmitterCreationParameters Python Definition
	class_<ParticleEmitterCreationParameters>("ParticleEmitterCreationParameters")
		.add_property("x", &ParticleEmitterCreationParameters::getXPy, &ParticleEmitterCreationParameters::setXPy)
		.add_property("y", &ParticleEmitterCreationParameters::getYPy, &ParticleEmitterCreationParameters::setYPy)
		.add_property("layer", &ParticleEmitterCreationParameters::getLayerPy, &ParticleEmitterCreationParameters::setLayerPy)
		.add_property("renderOrder", &ParticleEmitterCreationParameters::getRenderOrderPy, &ParticleEmitterCreationParameters::setRenderOrderPy)
		.add_property("name", &ParticleEmitterCreationParameters::getNamePy, &ParticleEmitterCreationParameters::setNamePy)
		.add_property("animationName", &ParticleEmitterCreationParameters::getAnimationNamePy, &ParticleEmitterCreationParameters::setAnimationNamePy)
		.add_property("interval", &ParticleEmitterCreationParameters::getIntervalPy, &ParticleEmitterCreationParameters::setIntervalPy)
		.add_property("particleLifespan", &ParticleEmitterCreationParameters::getParticleLifespanPy, &ParticleEmitterCreationParameters::setParticleLifespanPy)
		.add_property("attachParticles", &ParticleEmitterCreationParameters::getAttachParticlesPy, &ParticleEmitterCreationParameters::setAttachParticlesPy)
		.add_property("automatic", &ParticleEmitterCreationParameters::getAutomaticPy, &ParticleEmitterCreationParameters::setAutomaticPy)
		.add_property("maxParticles", &ParticleEmitterCreationParameters::getMaxParticlesPy, &ParticleEmitterCreationParameters::setMaxParticlesPy)
		.add_property("particlesPerEmission", &ParticleEmitterCreationParameters::getParticlesPerEmissionPy, &ParticleEmitterCreationParameters::setParticlesPerEmissionPy)
		//.add_property("roomId", &ParticleEmitterCreationParameters::getRoomIdPy, &ParticleEmitterCreationParameters::setRoomIdPy) I think this gets set automatically?
		.add_property("particleEmitterId", &ParticleEmitterCreationParameters::getParticleEmitterIdPy, &ParticleEmitterCreationParameters::setParticleEmitterIdPy)
		.add_property("particleId", &ParticleEmitterCreationParameters::getParticleIdPy, &ParticleEmitterCreationParameters::setParticleIdPy);

	// ParticleController Python Definition
	class_<ParticleController, boost::shared_ptr<ParticleController>, bases<EntityController>>("ParticleController")
		.add_property("hueColor", &ParticleController::getHueColorPy)
		.add_property("rotationAngle", &ParticleController::getRotationAnglePy, &ParticleController::setRotationAnglePy)
		.add_property("renderEffects", &ParticleController::getRenderEffectsPy)
		//.def("getRenderEffects", &ParticleController::getRenderEffectsPy) why did I use a def for this??
		.def("setAnimation", &ParticleController::setAnimationPy)
		.def("deactivate", &ParticleController::deactivatePy);

	// ParticleEmitterController Python Definition
	class_<ParticleEmitterController, boost::shared_ptr<ParticleEmitterController>, bases<EntityController>>("ParticleEmitterController")
		.add_property("activeParticleCount", &ParticleEmitterController::getActiveParticleCountPy)
		.add_property("particleCount", &ParticleEmitterController::getParticleCountPy)
		.def("getNextParticle", &ParticleEmitterController::getNextParticlePy)
		.def("getParticle", &ParticleEmitterController::getParticlePy)
		.def("emit", &ParticleEmitterController::emitPy)
		.def("setIsAutomatic", &ParticleEmitterController::setIsAutomaticPy);
		////.def("getNextParticle", make_function(&ParticleEmitterController::getNextParticle, return_internal_reference<>()))

	//// EntitySerializer Python Definition - Removed for now
	//class_<EntitySerializer>("EntitySerializer")
	//	.def("getThis", make_function(&EntitySerializer::getThisPy, return_internal_reference<>()));
	
	class_<Animation, boost::shared_ptr<Animation>>("Animation")
		.def("addFrame", &Animation::addFramePy)
		.def("getFrame", &Animation::getFramePy)
		.add_property("alphaMaskSheetId", &Animation::getAlphaMaskSheetIdPy, &Animation::setAlphaMaskSheetIdPy)
		.add_property("frameCount", &Animation::getFrameCountPy)
		.add_property("name", &Animation::getNamePy)
		.add_property("spriteSheetId", &Animation::getSpriteSheetIdPy, &Animation::setSpriteSheetIdPy);
		//.def("getFrame", &Animation::getFramePy, return_internal_reference<>())
	
	class_<AnimationFrame, boost::shared_ptr<AnimationFrame>>("AnimationFrame", init<int, int>())
		.def("addHitboxReference", &AnimationFrame::addHitboxReferencePy)
		.def("getHitboxReference", &AnimationFrame::getHitboxReferencePy)
		.def("getAnchorPoint", &AnimationFrame::getAnchorPointPy)
		.add_property("hitboxCount", &AnimationFrame::getHitboxCountPy)
		.add_property("anchorPointCount", &AnimationFrame::getAnchorPointCountPy)
		.add_property("spriteSheetCellColumn", &AnimationFrame::getSpriteSheetCellColumnPy)
		.add_property("spriteSheetCellRow", &AnimationFrame::getSpriteSheetCellRowPy);

	class_<AnimationManager>("AnimationManager")
		.def("getAnimation", &AnimationManager::getAnimationByNamePy)
		.def("getAnimationByIndex", &AnimationManager::getAnimationByIndexPy);

	class_<AnimationPlayer, boost::shared_ptr<AnimationPlayer>>("AnimationPlayer", init<int, AnimationStyle>())
		.def("updateAnimation", &AnimationPlayer::updateAnimationPy)
		.add_property("frameIndex", &AnimationPlayer::getCurrentFramePy, &AnimationPlayer::setCurrentFramePy)
		.add_property("framesPerSecond", &AnimationPlayer::getFramesPerSecondPy, &AnimationPlayer::setFramesPerSecondPy);

	// I don't think the user needs to be interact with this directly.
	//class_<AnchorPointManager>("AnchorPointManager")
	//	.def("getAnchorPoint", &AnchorPointManager::getAnchorPointPy);

	class_<AnchorPoint, AnchorPointPtr>("AnchorPoint", init<std::string, int, int>())
		.add_property("x", &AnchorPoint::getXPy)
		.add_property("y", &AnchorPoint::getYPy)
		.add_property("name", &AnchorPoint::getNamePy);
	
	// Hitbox Manager Python Definitionfsa
	class_<HitboxManager>("HitboxManager")
		.def("getHitbox", &HitboxManager::getHitboxPy);
		//.def("getHitbox", &HitboxManager::getHitboxPy, return_internal_reference<>());

	// Hitbox Python Definition
	class_<Hitbox, boost::shared_ptr<Hitbox>>("Hitbox", init<int, int, int, int>())
		.add_property("collisionRect", &Hitbox::getCollisionRectPy, &Hitbox::setCollisionRectPy)
		.add_property("x", &Hitbox::getLeftPy)
		.add_property("y", &Hitbox::getTopPy)
		.add_property("height", &Hitbox::getHeightPy)
		.add_property("width", &Hitbox::getWidthPy)
		.add_property("identity", &Hitbox::getIdentityPy)
		.add_property("collisionStyle", &Hitbox::getCollisionStylePy)
		.add_property("ownerPosition", &Hitbox::getOwnerPositionPy);

	// Rect Python Defintion
	class_<Rect>("Rect")
		.add_property("x", &Rect::getXPy, &Rect::setXPy)
		.add_property("y", &Rect::getYPy, &Rect::setYPy)
		.add_property("width", &Rect::getWPy, &Rect::setWPy)
		.add_property("height", &Rect::getHPy, &Rect::setHPy);
	
	// ColorRgba Python Defintion
	class_<ColorRgba, ColorRgbaPtr>("ColorRgba", init<float, float, float, float>())
		.add_property("r", &ColorRgba::getRPy, &ColorRgba::setRPy)
		.add_property("g", &ColorRgba::getGPy, &ColorRgba::setGPy)
		.add_property("b", &ColorRgba::getBPy, &ColorRgba::setBPy)
		.add_property("a", &ColorRgba::getAPy, &ColorRgba::setAPy)
		.def("create", &ColorRgba::createPy);

	// CollisionData Python Definition
	class_<CollisionData, boost::shared_ptr<CollisionData>>("CollisionData")
		.add_property("faceNormal", &CollisionData::getFaceNormalPy)
		.add_property("collidingEntityType", &CollisionData::getCollidingEntityTypePy, &CollisionData::setCollidingEntityTypePy)
		.add_property("solidCollision", &CollisionData::getSolidCollisionPy)
		.add_property("pythonObject", &CollisionData::getPythonInstance)
		.def("getMyHitbox", &CollisionData::getMyHitboxPy)
		.def("getCollidingHitbox", &CollisionData::getCollidingHitboxPy)
		.def("getCollidingEntityController", &CollisionData::getCollidingEntityControllerPy)
		.def("getCollidingEntityStateMachineController", &CollisionData::getCollidingEntityStateMachineControllerPy)
		.def("getCollidingEntityDynamicsController", make_function(&CollisionData::getCollidingEntityDynamicsControllerPy, return_internal_reference<>()))
		.def("setValue", &CollisionData::setValuePy)
		.def("getValue", &CollisionData::getValuePy);
		//.def("getThis", &CollisionData::getThisPy, return_internal_reference<>());
		//.def("getMyHitbox", make_function(&CollisionData::getMyHitboxPy, return_internal_reference<>()))
		//.def("getCollidingHitbox", make_function(&CollisionData::getCollidingHitboxPy, return_internal_reference<>()))
		//.def("getCollidingEntityController", make_function(&CollisionData::getCollidingEntityControllerPy, return_internal_reference<>()))
		//.def("getCollidingEntityStateMachineController", make_function(&CollisionData::getCollidingEntityStateMachineControllerPy, return_internal_reference<>()))

	// TileCollisionData Python Definition
	class_<TileCollisionData, boost::shared_ptr<TileCollisionData>, bases<CollisionData>>("TileCollisionData")
		.add_property("faceNormal", &TileCollisionData::getFaceNormalPy)
		.add_property("collidingEntityType", &TileCollisionData::getCollidingEntityTypePy, &TileCollisionData::setCollidingEntityTypePy)
		.add_property("solidCollision", &TileCollisionData::getSolidCollisionPy)
		.add_property("tileGroupId", &TileCollisionData::getTileGroupIdPy, &TileCollisionData::setTileGroupIdPy)
		.def("getMyHitbox", &TileCollisionData::getMyHitboxPy)
		.def("getCollidingHitbox", &TileCollisionData::getCollidingHitboxPy)
		.def("getCollidingEntityController", &TileCollisionData::getCollidingEntityControllerPy)
		.def("getCollidingEntityStateMachineController", &TileCollisionData::getCollidingEntityStateMachineControllerPy)
		.def("setValue", &TileCollisionData::setValuePy)
		.def("getValue", &TileCollisionData::getValuePy);
		//.def("getThis", &TileCollisionData::getThisPy, return_internal_reference<>());
		//.def("getMyHitbox", make_function(&TileCollisionData::getMyHitboxPy, return_internal_reference<>()))
		//.def("getCollidingHitbox", make_function(&TileCollisionData::getCollidingHitboxPy, return_internal_reference<>()))
		//.def("getCollidingEntityController", make_function(&TileCollisionData::getCollidingEntityControllerPy, return_internal_reference<>()))
		//.def("getCollidingEntityStateMachineController", make_function(&TileCollisionData::getCollidingEntityStateMachineControllerPy, return_internal_reference<>()))

	// InputDeviceManager Python Definition
	class_<InputDeviceManager, boost::noncopyable>("InputDeviceManager", no_init)
		.add_property("inputDeviceCount", &InputDeviceManager::getInputDeviceCountPy)
		.add_property("blockEntityInput", &InputDeviceManager::getBlockEntityInputPy, &InputDeviceManager::setBlockEntityInputPy)
		.def("getInputDeviceByChannel", &InputDeviceManager::getInputDeviceWrapperByChannelPy)
		.def("getInputDeviceByIndex", &InputDeviceManager::getInputDeviceWrapperByIndexPy)
		.def("getInputDeviceByName", &InputDeviceManager::getInputDeviceWrapperByNamePy)
		.def("getGameButtonManager", &InputDeviceManager::getGameButtonManagerPy)
		.def("changeInputChannelOfListeners", &InputDeviceManager::changeInputChannelOfListenersPy)
		.def("setDefaultInputChannel", &InputDeviceManager::setDefaultInputChannelPy)
		.def("disableEntityInput", &InputDeviceManager::disableEntityInputPy)
		.def("enableEntityInput", &InputDeviceManager::enableEntityInputPy)
		.def("disableUiInput", &InputDeviceManager::disableUiInputPy)
		.def("enableUiInput", &InputDeviceManager::enableUiInputPy);
		//.def("getInputDeviceByChannel", make_function(&InputDeviceManager::getInputDeviceWrapperByChannelPy, return_internal_reference<>()))
		//.def("getInputDeviceByIndex", make_function(&InputDeviceManager::getInputDeviceWrapperByIndexPy, return_internal_reference<>()))
		//.def("getInputDeviceByName", make_function(&InputDeviceManager::getInputDeviceWrapperByNamePy, return_internal_reference<>()))
		//.def("getGameButtonManager", make_function(&InputDeviceManager::getGameButtonManagerPy, return_internal_reference<>()))
	
	// InputDevice Python Definition
	class_<InputDeviceWrapper, boost::shared_ptr<InputDeviceWrapper>>("InputDevice")
		.add_property("isInitialized", &InputDeviceWrapper::getIsInitializedPy)
		.add_property("deviceName", &InputDeviceWrapper::getDeviceNamePy)
		.add_property("isConfiguring", &InputDeviceWrapper::isConfiguringPy)
		.add_property("channel", &InputDeviceWrapper::getChannelPy)
		.add_property("isBlocked", &InputDeviceWrapper::getIsBlockedPy, &InputDeviceWrapper::setIsBlockedPy)
		.def("getButtonState",  &InputDeviceWrapper::getButtonStatePy)
		.def("configureButton",  &InputDeviceWrapper::configureButtonPy)
		.def("getDeviceButtonName",  &InputDeviceWrapper::getDeviceButtonNamePy)
		.def("getDeviceButtonCode",  &InputDeviceWrapper::getDeviceButtonCodePy)
		.def("setKeyValue", &InputDeviceWrapper::setKeyValuePy)
		.def("mapDeviceButtonCode", &InputDeviceWrapper::mapDeviceButtonCodePy);

	// GameButtonManager Python Defintion
	class_<GameButtonManager, boost::shared_ptr<GameButtonManager>>("GameButtonManager")
		.add_property("buttonCount", &GameButtonManager::getGameButtonCountPy)
		.add_property("buttonGroupCount", &GameButtonManager::getGameButtonGroupCountPy)
		.def("getButtonCountForGroup", &GameButtonManager::getGameButtonCountForGroupPy)
		.def("getButtonId", &GameButtonManager::getGameButtonIdPy)
		.def("getButtonIdForGroup", &GameButtonManager::getGameButtonIdForGroupPy)
		.def("getButtonUuid",  &GameButtonManager::getGameButtonUuidStringPy)
		.def("getButtonName", &GameButtonManager::getGameButtonNamePy)
		.def("getButtonLabel", &GameButtonManager::getGameButtonLabelPy)
		.def("getButtonGroupId", &GameButtonManager::getGameButtonGroupIdPy)
		.def("getButtonGroupName", &GameButtonManager::getGameButtonGroupNamePy)
		.def("getButtonGroupIdForButton", &GameButtonManager::getGameButtonGroupIdForButtonPy);
	
	// BaseIds Python Defintion
	class_<BaseIds>("ids")
		.def("getIntegerFromUuid", &BaseIds::getIntegerFromUuidStringPy)
		.def("getIdFromName", &BaseIds::getIdFromNamePy)
		.staticmethod("getIntegerFromUuid");

	class_<RenderEffects, boost::shared_ptr<RenderEffects>>("RenderEffects")
		.add_property("alphaGradientDirection", &RenderEffects::getAlphaGradientDirectionPy, &RenderEffects::setAlphaGradientDirectionPy)
		.add_property("alphaGradientFrom", &RenderEffects::getAlphaGradientFromPy, &RenderEffects::setAlphaGradientFromPy)
		.add_property("alphaGradientTo", &RenderEffects::getAlphaGradientToPy, &RenderEffects::setAlphaGradientToPy)
		.add_property("alphaGradientRadius", &RenderEffects::getAlphaGradientRadiusPy, &RenderEffects::setAlphaGradientRadiusPy)
		.add_property("blendColor", &RenderEffects::getBlendColorPy)
		.add_property("blendPercent", &RenderEffects::getBlendPercentPy)
		.add_property("alphaGradientRadialCenter", &RenderEffects::getAlphaGradientRadialCenterPointPy)
		.add_property("extentLeft", &RenderEffects::getExtentLeftPy, &RenderEffects::setExtentLeftPy)
		.add_property("extentRight", &RenderEffects::getExtentRightPy, &RenderEffects::setExtentRightPy)
		.add_property("extentTop", &RenderEffects::getExtentTopPy, &RenderEffects::setExtentTopPy)
		.add_property("extentBottom", &RenderEffects::getExtentBottomPy, &RenderEffects::setExtentBottomPy)
		.add_property("hueColor", &RenderEffects::getHueColorPy)
		.add_property("interpolateExtents", &RenderEffects::getInterpolateExtentsPy, &RenderEffects::setInterpolateExtentsPy)
		.add_property("interpolateRotation", &RenderEffects::getInterpolateRotationPy, &RenderEffects::setInterpolateRotationPy)
		.add_property("mirrorHorizontal", &RenderEffects::getMirrorHorizontalPy, &RenderEffects::setMirrorHorizontalPy)
		.add_property("mirrorHorizontal", &RenderEffects::getMirrorHorizontalPy, &RenderEffects::setMirrorHorizontalPy)
		.add_property("outlineColor", &RenderEffects::getOutlineColorPy)
		.def("addRotation", &RenderEffects::addRotationPy)
		.def("getAlphaMask", &RenderEffects::getAlphaMaskPy);
		//.add_property("rotation", &RenderEffects::getRotationAnglePy)
		//.add_property("pivotPoint", &RenderEffects::getPivotPointPy);

	class_<AlphaMask, boost::shared_ptr<AlphaMask>>("AlphaMask")
		.add_property("sheetCellColumn", &AlphaMask::getSheetCellColumnPy, &AlphaMask::setSheetCellColumnPy)
		.add_property("sheetCellRow", &AlphaMask::getSheetCellRowPy, &AlphaMask::setSheetCellRowPy)
		.add_property("sheetId", &AlphaMask::getSheetIdPy, &AlphaMask::setSheetIdPy);

	// Renderer Python Definition

	// Function overloads
	void (Renderer::*renderSheetById)(float, float, int) = &Renderer::renderSheetByIdPy;
	void (Renderer::*renderSheetByIdWithEffects)(float, float, int, boost::shared_ptr<RenderEffects> renderEffects) = &Renderer::renderSheetByIdPy;

	void (Renderer::*renderSheetSection)(float, float, int, Rect) = &Renderer::renderSheetSectionPy;
	void (Renderer::*renderSheetSectionEffects)(float, float, int, Rect, boost::shared_ptr<RenderEffects> renderEffects) = &Renderer::renderSheetSectionPy;
			
	void (Renderer::*renderSheetCell)(float, float, int, int, int) = &Renderer::renderSheetCellPy;
	void (Renderer::*renderSheetCellWithEffects)(float, float, int, int, int, boost::shared_ptr<RenderEffects> renderEffects) = &Renderer::renderSheetCellPy;

	class_<Renderer, boost::noncopyable>("Renderer", no_init)
		.def("getSheet", &Renderer::getSheetPy)
		.def("getSheetByName", &Renderer::getSheetByNamePy)
		.def("getSheetIdByName", &Renderer::getSheetIDByNamePy)
		.def("renderSheet", renderSheetById)
		.def("renderSheet", renderSheetByIdWithEffects)
		.def("renderSheetCell", renderSheetCell)
		.def("renderSheetCell", renderSheetCellWithEffects)
		.def("renderSheetSection", renderSheetSection)
		.def("renderSheetSection", renderSheetSectionEffects)
		.def("getSheetIdByName", &Renderer::getSheetIDByNamePy)
		.def("getSheetIdByName", &Renderer::getSheetIDByNamePy)
		.def("drawRect", &Renderer::drawRectPy)
		.def("fillRect", &Renderer::fillRectPy)
		.def("setFadeColor", &Renderer::setFadeColorPy)
		.def("setFadeOpacity", &Renderer::setFadeOpacityPy)
		.add_property("enableFade", &Renderer::getEnableFadePy, &Renderer::setEnableFadePy)
		.add_property("screenHeight", &Renderer::getScreenHeightPy)
		.add_property("screenWidth", &Renderer::getScreenWidthPy)
		.add_property("fadeOpacity", &Renderer::getFadeOpacityPy, &Renderer::setFadeOpacityPy);
		//.def("getSheet", make_function(&Renderer::getSheetPy, return_internal_reference<>()))
		//.def("getSheetByName", make_function(&Renderer::getSheetByNamePy, return_internal_reference<>()))

	class_<SpriteSheet, boost::shared_ptr<SpriteSheet>, boost::noncopyable>("SpriteSheet", no_init)		
		.add_property("id", &SpriteSheet::getIdPy)
		.add_property("columns", &SpriteSheet::getColumnsPy)
		.add_property("rows", &SpriteSheet::getRowsPy)
		.add_property("cellHeight", &SpriteSheet::getCellHeightPy)
		.add_property("cellWidth", &SpriteSheet::getCellWidthPy)
		.add_property("scaleFactor", &SpriteSheet::getScaleFactorPy);
	
	// AudioPlayer Python Definition
	class_<AudioPlayer, boost::noncopyable>("AudioPlayer", no_init)
		.def("playAudio", &AudioPlayer::playAudioByNamePy)
		.def("stopAudio", &AudioPlayer::stopAudioByNamePy)
		.def("stopAllAudio", &AudioPlayer::stopAllAudioPy)
		.def("pauseAllAudio", &AudioPlayer::setPauseAllAudioPy)
		.def("setGroupVolume", &AudioPlayer::setGroupVolumePy)
		.def("getGroupVolume", &AudioPlayer::getGroupVolumePy)
		.def("isPlayingByIndex", &AudioPlayer::isAudioPlayingByIndexPy)
		.def("isPlayingByName", &AudioPlayer::isAudioPlayingByNamePy);
	
	// Game Timer Python Defintion
	class_<GameTimer, boost::noncopyable>("Timer", no_init)
		.add_property("timeElapsed", &GameTimer::getTimeElapsedPy)
		.add_property("pingTime", &GameTimer::getPingTimePy)
		.def("addTimer", &GameTimer::addTimerPy)
		.def("logTimeStart", &GameTimer::logTimeStartPy)
		.def("logTimeEnd", &GameTimer::logTimeEndPy);
	
	// TextManager Python Definition
	class_<TextManager>("TextManager", no_init)
		.def("addText", &TextManager::addTextPy);
	
	// FontManager Python Definition
	class_<FontManager>("FontManager", no_init)
		.def("getFont", &FontManager::getFontPy);
		//.def("getFont", make_function(&FontManager::getFontPy, return_internal_reference<>()));

	// RenderableText Python Definition
	class_<RenderableText, boost::shared_ptr<RenderableText>>("RenderableText")
		.add_property("displayText", &RenderableText::getDisplayTextPy, &RenderableText::setDisplayTextPy)
		.add_property("fontName", &RenderableText::getFontNamePy, &RenderableText::setFontNamePy)
		.add_property("x", &RenderableText::getXPy, &RenderableText::setXPy)
		.add_property("y", &RenderableText::getYPy, &RenderableText::setYPy)
		.add_property("layer", &RenderableText::getLayerIndexPy, &RenderableText::setLayerIndexPy)
		.add_property("interpolatePosition", &RenderableText::getInterpolatePositionPy, &RenderableText::setInterpolatePositionPy)
		.add_property("color", &RenderableText::getColorPy);
		//.def("getThis", make_function(&RenderableText::getThisPy, return_internal_reference<>()));
		//.add_property("color",  make_function(&RenderableText::getColorPy, return_internal_reference<>()))

	// RenderableTextController Python Definition
	class_<RenderableTextController>("RenderableTextController")
		.add_property("isActive", &RenderableTextController::getIsActivePy)
		.def("remove", &RenderableTextController::removePy);

	// BasicRenderableText Python Definition
	class_<BasicRenderableText, boost::shared_ptr<BasicRenderableText>, bases<RenderableText>>("BasicRenderableText")
		.add_property("displayText", &BasicRenderableText::getDisplayTextPy, &BasicRenderableText::setDisplayTextPy)
		.add_property("fontName", &BasicRenderableText::getFontNamePy, &BasicRenderableText::setFontNamePy)
		.add_property("x", &BasicRenderableText::getXPy, &BasicRenderableText::setXPy)
		.add_property("y", &BasicRenderableText::getYPy, &BasicRenderableText::setYPy)
		.add_property("layer", &BasicRenderableText::getLayerIndexPy, &BasicRenderableText::setLayerIndexPy)
		.add_property("interpolatePosition", &BasicRenderableText::getInterpolatePositionPy, &BasicRenderableText::setInterpolatePositionPy)
		.add_property("color", &BasicRenderableText::getColorPy);
		//.def("getThis", make_function(&RenderableText::getThisPy, return_internal_reference<>()));
		//.add_property("color", make_function(&BasicRenderableText::getColorPy, return_internal_reference<>()))
	
	// BitmapFont Python Definition

	void (BitmapFont::*writeText)(int, int, std::string, double, double, double, double) = &BitmapFont::writeTextPy;
	void (BitmapFont::*writeTextFloat)(float, float, std::string, double, double, double, double) = &BitmapFont::writeTextPy;
	void (BitmapFont::*writeTextWithEffects)(int, int, std::string, boost::shared_ptr<RenderEffects> renderEffects) = &BitmapFont::writeTextPy;
	void (BitmapFont::*writeTextFloatWithEffects)(float, float, std::string, boost::shared_ptr<RenderEffects> renderEffects) = &BitmapFont::writeTextPy;

	class_<BitmapFont, boost::shared_ptr<BitmapFont>>("BitmapFont", no_init)
		.add_property("characterHeight", &BitmapFont::getCharacterHeightPy)
		.add_property("characterWidth", &BitmapFont::getCharacterWidthPy)
		.def("writeText", writeText)
		.def("writeText", writeTextFloat)
		.def("writeText", writeTextWithEffects)
		.def("writeText", writeTextFloatWithEffects);

	// Messenger Python Definition
	class_<Messenger, boost::noncopyable>("Messenger")
		.def("sendMessage", &Messenger::sendMessagePy);

	// Message Python Defintion
	class_<Message>("Message")
		.add_property("code", &Message::getMessageCodePy, &Message::setMessageCodePy)
		.add_property("senderId", &Message::getSenderIdPy, &Message::setSenderIdPy)
		.add_property("senderEntityType", &Message::getSenderTypePy, &Message::setSenderTypePy)
		.add_property("roomId", &Message::getRoomIdPy, &Message::setRoomIdPy)
		.add_property("priority", &Message::getPriorityPy, &Message::setPriorityPy)
		.add_property("receiverIdCount", &Message::getReceiverIdCountPy)
		.add_property("receiverEntityTypeCount", &Message::getReceiverEntityTypeCountPy)
		.def("addReceiverId", &Message::addReceiverIdPy)
		.def("addReceiverEntityType", &Message::addReceiverEntityTypePy)
		.def("getReceiverId", &Message::getReceiverIdPy)
		.def("getReceiverEntityType", &Message::getReceiverEntityTypePy)
		.def("getMessageContent", &Message::getMessageContentPy)
		.def("setMessageContent", &Message::setMessageContentPy);
		//.def("getMessageContent", make_function(&Message::getMessageContentPy, return_internal_reference<>()))

	// Message Content Python Defintion
	class_<MessageContent, boost::shared_ptr<MessageContent>>("MessageContent")
		.add_property("pythonObject", &MessageContent::getPyInstance);
		//.def("getThis",&MessageContent::getThisPy);
		//.def("getThis", make_function(&MessageContent::getThisPy, return_internal_reference<>()));

	// Query Manager Python Definition
	class_<QueryManager, boost::noncopyable>("QueryManager")
		.def("getQuery", &QueryManager::getQueryPy)
		.def("runQuery", &QueryManager::runQueryPy)
		.def("closeQuery", &QueryManager::closeQueryPy);
	
	// Query Container Python Definition
	class_<QueryContainer>("QueryContainer", init<QueryId>())
		.add_property("parameters", &QueryContainer::getParametersPy)
		.add_property("result", &QueryContainer::getResultPy);

	// Query Result Python Definition
	class_<QueryResult, boost::noncopyable>("QueryResult");

	// Query Parameters Python Definition
	class_<QueryParameters, boost::noncopyable>("QueryParameters");
	
	// Entity Metadata Container Definition
	class_<EntityMetadataContainer>("EntityMetadataContainer")
		.add_property("entityMetadataCount", &EntityMetadataContainer::getEntityMetadataCountPy)
		.def("getEntityMetadata", &EntityMetadataContainer::getEntityMetadataPy);

	// Entity Metadata Definition
	class_<EntityMetadata, boost::shared_ptr<EntityMetadata>>("EntityMetadata")
		.add_property("classificationId", &EntityMetadata::getClassificationIdPy)
		.add_property("typeId", &EntityMetadata::getEntityTypeIdPy)
		.add_property("instanceId", &EntityMetadata::getEntityInstanceIdPy)
		.add_property("instanceName", &EntityMetadata::getEntityInstanceNamePy)
		.add_property("mapLayer", &EntityMetadata::getMapLayerPy)
		.add_property("room", &EntityMetadata::getRoomMetadataPy)
		.add_property("previousRoom", &EntityMetadata::getPreviousRoomMetadataPy)
		.add_property("height", &EntityMetadata::getHeightPy)
		.add_property("width", &EntityMetadata::getWidthPy)
		.add_property("tag", &EntityMetadata::getTagPy);

	// Size Definition
	class_<Size, boost::shared_ptr<Size>>("Size", init<int, int>())
		.add_property("height", &Size::getHeightPy, &Size::setHeightPy)
		.add_property("width", &Size::getWidthPy, &Size::setWidthPy);

	// Ui Definition
	class_<Ui, boost::shared_ptr<Ui>>("Ui")
		.def("getPanelByName", &Ui::getPanelByNamePy)
		.def("getWidgetByName", &Ui::getWidgetByNamePy)
		.def("getFocusedWidget", &Ui::getFocusedWidgetPy)
		.def("selectElement", &Ui::selectElementPy)
		.def("focusFirstElement", &Ui::focusFirstElementPy)
		.def("focusPreviousElement", &Ui::focusPreviousElementPy)
		.def("focusNextElement", &Ui::focusNextElementPy)
		.def("focusLastElement", &Ui::focusLastElementPy)
		.def("focusElement", &Ui::focusElementPy)
		.def("showPanel", &Ui::showPanelPy)
		.def("hidePanel", &Ui::hidePanelPy)
		.def("showWidget", &Ui::showWidgetPy)
		.def("hideWidget", &Ui::hideWidgetPy)
		.def("focusElement", &Ui::focusElementPy)
		.def("createUiPanelElement", &Ui::createUiPanelElementPy)
		.def("writePanelTree", &Ui::writePanelTreePy)
		.def("getPanelSize", &Ui::getPanelSizePy);

	// Ui Panel Defintion
	class_<UiPanel, UiPanelPtr>("UiPanel", init<std::string>())		
		.add_property("childElementCount", &UiPanel::getChildElementCountPy)
		.add_property("layoutStyle", &UiPanel::getLayoutStylePy, &UiPanel::setLayoutStylePy)
		.add_property("isVisible", &UiPanel::getIsVisiblePy)
		.add_property("isFocusable", &UiPanel::getIsFocusablePy)
		.add_property("name", &UiPanel::getNamePy)
		.add_property("position", &UiPanel::getPositionPy)
		.add_property("size", &UiPanel::getSizePy)
		.add_property("parentPanel", &UiPanel::getParentPanelPy)
		.add_property("background", &UiPanel::getBackgroundPy, &UiPanel::setBackgroundPy)
		.def("getChildPanelByName", &UiPanel::getChildPanelByNamePy);

	// Ui Widget Defintion
	class_<UiWidget, UiWidgetPtr>("UiWidget", init<std::string>())
		.add_property("isVisible", &UiWidget::getIsVisiblePy)
		.add_property("isFocusable", &UiWidget::getIsFocusablePy)
		.add_property("name", &UiWidget::getNamePy)
		.add_property("parentPanel", &UiWidget::getParentPanelPy)
		.add_property("pythonObject", &UiWidget::getPyInstance);

	// Ui Panel Element Definition Defintion
	class_<UiPanelElementDefinition, boost::shared_ptr<UiPanelElementDefinition>>("UiPanelElementDefinition")
		.add_property("backgroundSheet", &UiPanelElementDefinition::getBackgroundSheetNamePy, &UiPanelElementDefinition::setBackgroundSheetNamePy)
		.add_property("borderBottom", &UiPanelElementDefinition::getBorderBottomPy, &UiPanelElementDefinition::setBorderBottomPy)
		.add_property("borderLeft", &UiPanelElementDefinition::getBorderLeftPy, &UiPanelElementDefinition::setBorderLeftPy)
		.add_property("borderRight", &UiPanelElementDefinition::getBorderRightPy, &UiPanelElementDefinition::setBorderRightPy)
		.add_property("borderTop", &UiPanelElementDefinition::getBorderTopPy, &UiPanelElementDefinition::setBorderTopPy)
		.add_property("buttonDownHandler", &UiPanelElementDefinition::getButtonDownHandlerPy, &UiPanelElementDefinition::setButtonDownHandlerPy)
		.add_property("buttonUpHandler", &UiPanelElementDefinition::getButtonUpHandlerPy, &UiPanelElementDefinition::setButtonUpHandlerPy)
		.add_property("caption", &UiPanelElementDefinition::getCaptionPy, &UiPanelElementDefinition::setCaptionPy)
		.add_property("captionColorBlue", &UiPanelElementDefinition::getCaptionColorBluePy, &UiPanelElementDefinition::setCaptionColorBluePy)
		.add_property("captionColorGreen", &UiPanelElementDefinition::getCaptionColorGreenPy, &UiPanelElementDefinition::setCaptionColorGreenPy)
		.add_property("captionColorRed", &UiPanelElementDefinition::getCaptionColorRedPy, &UiPanelElementDefinition::setCaptionColorRedPy)
		.add_property("captionFont", &UiPanelElementDefinition::getCaptionFontPy, &UiPanelElementDefinition::setCaptionFontPy)
		.add_property("captionPositionLeft", &UiPanelElementDefinition::getCaptionPositionLeftPy, &UiPanelElementDefinition::setCaptionPositionLeftPy)
		.add_property("captionPositionTop", &UiPanelElementDefinition::getCaptionPositionTopPy, &UiPanelElementDefinition::setCaptionPositionTopPy)
		.add_property("captionScale", &UiPanelElementDefinition::getCaptionScalePy, &UiPanelElementDefinition::setCaptionScalePy)
		.add_property("controlFlow", &UiPanelElementDefinition::getControlFlowPy, &UiPanelElementDefinition::setControlFlowPy)
		.add_property("fillBottom", &UiPanelElementDefinition::getFillBottomPy, &UiPanelElementDefinition::setFillBottomPy)
		.add_property("fillLeft", &UiPanelElementDefinition::getFillLeftPy, &UiPanelElementDefinition::setFillLeftPy)
		.add_property("fillRight", &UiPanelElementDefinition::getFillRightPy, &UiPanelElementDefinition::setFillRightPy)
		.add_property("fillTop", &UiPanelElementDefinition::getFillTopPy, &UiPanelElementDefinition::setFillTopPy)
		.add_property("focusable", &UiPanelElementDefinition::getFocusablePy, &UiPanelElementDefinition::setFocusablePy)
		.add_property("focusWrap", &UiPanelElementDefinition::getFocusWrapPy, &UiPanelElementDefinition::setFocusWrapPy)
		.add_property("frameMarginBottom", &UiPanelElementDefinition::getFrameMarginBottomPy, &UiPanelElementDefinition::setFrameMarginBottomPy)
		.add_property("frameMarginLeft", &UiPanelElementDefinition::getFrameMarginLeftPy, &UiPanelElementDefinition::setFrameMarginLeftPy)
		.add_property("frameMarginRight", &UiPanelElementDefinition::getFrameMarginRightPy, &UiPanelElementDefinition::setFrameMarginRightPy)
		.add_property("frameMarginTop", &UiPanelElementDefinition::getFrameMarginTopPy, &UiPanelElementDefinition::setFrameMarginTopPy)
		.add_property("gotFocusHandler", &UiPanelElementDefinition::getGotFocusHandlerPy, &UiPanelElementDefinition::setGotFocusHandlerPy)
		.add_property("hiddenHandler", &UiPanelElementDefinition::getHiddenHandlerPy, &UiPanelElementDefinition::setHiddenHandlerPy)
		.add_property("horizontalAlignment", &UiPanelElementDefinition::getHorizontalAlignmentPy, &UiPanelElementDefinition::setHorizontalAlignmentPy)
		.add_property("layout", &UiPanelElementDefinition::getLayoutPy, &UiPanelElementDefinition::setLayoutPy)
		.add_property("lostFocusHandler", &UiPanelElementDefinition::getLostFocusHandlerPy, &UiPanelElementDefinition::setLostFocusHandlerPy)
		.add_property("marginBottom", &UiPanelElementDefinition::getMarginBottomPy, &UiPanelElementDefinition::setMarginBottomPy)
		.add_property("marginLeft", &UiPanelElementDefinition::getMarginLeftPy, &UiPanelElementDefinition::setMarginLeftPy)
		.add_property("marginRight", &UiPanelElementDefinition::getMarginRightPy, &UiPanelElementDefinition::setMarginRightPy)
		.add_property("marginTop", &UiPanelElementDefinition::getMarginTopPy, &UiPanelElementDefinition::setMarginTopPy)
		.add_property("name", &UiPanelElementDefinition::getNamePy, &UiPanelElementDefinition::setNamePy)
		.add_property("paddingBottom", &UiPanelElementDefinition::getPaddingBottomPy, &UiPanelElementDefinition::setPaddingBottomPy)
		.add_property("paddingLeft", &UiPanelElementDefinition::getPaddingLeftPy, &UiPanelElementDefinition::setPaddingLeftPy)
		.add_property("paddingRight", &UiPanelElementDefinition::getPaddingRightPy, &UiPanelElementDefinition::setPaddingRightPy)
		.add_property("paddingTop", &UiPanelElementDefinition::getPaddingTopPy, &UiPanelElementDefinition::setPaddingTopPy)
		.add_property("elementType", &UiPanelElementDefinition::getPanelElementTypePy, &UiPanelElementDefinition::setPanelElementTypePy)
		.add_property("params", &UiPanelElementDefinition::getParamsPy, &UiPanelElementDefinition::setParamsPy)
		.add_property("positionStyle", &UiPanelElementDefinition::getPositionStylePy, &UiPanelElementDefinition::setPositionStylePy)
		.add_property("selectElementHandler", &UiPanelElementDefinition::getSelectElementHandlerPy, &UiPanelElementDefinition::setSelectElementHandlerPy)
		.add_property("shownHandler", &UiPanelElementDefinition::getShownHandlerPy, &UiPanelElementDefinition::setShownHandlerPy)
		.add_property("type", &UiPanelElementDefinition::getTypePy, &UiPanelElementDefinition::setTypePy)
		.add_property("verticalAlignment", &UiPanelElementDefinition::getVerticalAlignmentPy, &UiPanelElementDefinition::setVerticalAlignmentPy)
		.add_property("visible", &UiPanelElementDefinition::getVisiblePy, &UiPanelElementDefinition::setVisiblePy)
		.def("addElement", &UiPanelElementDefinition::addElementPy);

	// Particle Metadata Definition
	class_<ParticleMetadata, boost::shared_ptr<ParticleMetadata>, bases<EntityMetadata>>("ParticleMetadata")
		.add_property("lifetime", &ParticleMetadata::getLifetimePy);

	// Room Metadata Definition
	class_<RoomMetadata, boost::shared_ptr<RoomMetadata>>("RoomMetadata")
		.add_property("id", &RoomMetadata::getRoomIdPy)
		.add_property("name", &RoomMetadata::getRoomNamePy)
		.add_property("mapHeight", &RoomMetadata::getMapHeightPy)
		.add_property("mapWidth", &RoomMetadata::getMapWidthPy);

    class_<std::map<std::string, std::string>>("StringMap")
		.def(map_indexing_suite<std::map<std::string, std::string> >());

	class_<EntityTypeMap>("EntityTypeMap")
	  .def("__len__", &EntityTypeMap::sizePy)
	  .def("__getitem__", &EntityTypeMap::getValuePy);

	class_<EntityList>("EntityList")
	  .def("__len__", &EntityList::sizePy)
	  .def("__getitem__", &EntityList::getValuePy);
		
	// Position Python Defintion
	class_<Position, boost::shared_ptr<Position>>("Position", init<int, int>())
		.add_property("x", &Position::getXPy, &Position::setXPy)
		.add_property("y", &Position::getYPy, &Position::setYPy)
		.add_property("previousX", &Position::getPreviousXPy, &Position::setPreviousXPy)
		.add_property("previousY", &Position::getPreviousYPy, &Position::setPreviousYPy);


	// Debugger Python Definition
	class_<Debugger, boost::shared_ptr<Debugger>>("Debugger", no_init)
		.add_property("isSingleFrameUpdate", &Debugger::getIsSingleFrameUpdatePy)
		.def("appendToLog", &Debugger::appendToLogPy);
}

/* 
** Known bug in VS2015 update 3. Need to explicitly specify conversion to pointer.
** See: https://stackoverflow.com/questions/38261530/unresolved-external-symbols-since-visual-studio-2015-update-3-boost-python-link
*/
namespace boost
{
	template <>
	AnchorPoint const volatile * get_pointer<class AnchorPoint const volatile >(
		class AnchorPoint const volatile *value)
	{
		return value;
	}

	template <>
	AnchorPointManager const volatile * get_pointer<class AnchorPointManager const volatile >(
		class AnchorPointManager const volatile *value)
	{
		return value;
	}

	template <>
	AlphaMask const volatile * get_pointer<class AlphaMask const volatile >(
		class AlphaMask const volatile *value)
	{
		return value;
	}

	template <>
	Animation const volatile * get_pointer<class Animation const volatile >(
		class Animation const volatile *value)
	{
		return value;
	}

	template <>
	AnimationFrame const volatile * get_pointer<class AnimationFrame const volatile >(
		class AnimationFrame const volatile *value)
	{
		return value;
	}

	template <>
	AnimationManager const volatile * get_pointer<class AnimationManager const volatile >(
		class AnimationManager const volatile *value)
	{
		return value;
	}

	template <>
	AnimationPlayer const volatile * get_pointer<class AnimationPlayer const volatile >(
		class AnimationPlayer const volatile *value)
	{
		return value;
	}

	template <>
	AudioPlayer const volatile * get_pointer<class AudioPlayer const volatile >(
		class AudioPlayer const volatile *value)
	{
		return value;
	}

	template <>
	AudioSource const volatile * get_pointer<class AudioSource const volatile >(
		class AudioSource const volatile *value)
	{
		return value;
	}

	template <>
	AudioSourceProperties const volatile * get_pointer<class AudioSourceProperties const volatile >(
		class AudioSourceProperties const volatile *value)
	{
		return value;
	}
	
	template <>
	BaseIds const volatile * get_pointer<class BaseIds const volatile >(
		class BaseIds const volatile *value)
	{
		return value;
	}

	template <>
	BasicRenderableText const volatile * get_pointer<class BasicRenderableText const volatile >(
		class BasicRenderableText const volatile *value)
	{
		return value;
	}

	template <>
	BitmapFont const volatile * get_pointer<class BitmapFont const volatile >(
		class BitmapFont const volatile *value)
	{
		return value;
	}

	template <>
	CameraController const volatile * get_pointer<class CameraController const volatile >(
		class CameraController const volatile *value)
	{
		return value;
	}

	template <>
	CameraManager const volatile * get_pointer<class CameraManager const volatile >(
		class CameraManager const volatile *value)
	{
		return value;
	}

	template <>
	CollisionData const volatile * get_pointer<class CollisionData const volatile >(
		class CollisionData const volatile *value)
	{
		return value;
	}

	template <>
	ColorRgba const volatile * get_pointer<class ColorRgba const volatile >(
		class ColorRgba const volatile *value)
	{
		return value;
	}

	template <>
	Debugger const volatile * get_pointer<class Debugger const volatile >(
		class Debugger const volatile *value)
	{
		return value;
	}

	template <>
	DynamicsController const volatile * get_pointer<class DynamicsController const volatile >(
		class DynamicsController const volatile *value)
	{
		return value;
	}

	template <>
	EngineConfig const volatile * get_pointer<class EngineConfig const volatile >(
		class EngineConfig const volatile *value)
	{
		return value;
	}

	template <>
	EngineController const volatile * get_pointer<class EngineController const volatile >(
		class EngineController const volatile *value)
	{
		return value;
	}

	template <>
	EntityController const volatile * get_pointer<class EntityController const volatile >(
		class EntityController const volatile *value)
	{
		return value;
	}

	template <>
	EntityMetadata const volatile * get_pointer<class EntityMetadata const volatile >(
		class EntityMetadata const volatile *value)
	{
		return value;
	}

	template <>
	EntityMetadataContainer const volatile * get_pointer<class EntityMetadataContainer const volatile >(
		class EntityMetadataContainer const volatile *value)
	{
		return value;
	}

	//template <>
	//EntitySerializer const volatile * get_pointer<class EntitySerializer const volatile >(
	//	class EntitySerializer const volatile *value)
	//{
	//	return value;
	//}

	template <>
	FontManager const volatile * get_pointer<class FontManager const volatile >(
		class FontManager const volatile *value)
	{
		return value;
	}

	//template <>
	//GameTimer const volatile * get_pointer<class GameTimer const volatile >(
	//	class GameTimer const volatile *value)
	//{
	//	return value;
	//}

	template <>
	Hitbox const volatile * get_pointer<class Hitbox const volatile >(
		class Hitbox const volatile *value)
	{
		return value;
	}

	template <>
	HitboxManager const volatile * get_pointer<class HitboxManager const volatile >(
		class HitboxManager const volatile *value)
	{
		return value;
	}

	template <>
	InputDeviceManager const volatile * get_pointer<class InputDeviceManager const volatile >(
		class InputDeviceManager const volatile *value)
	{
		return value;
	}

	template <>
	InputDeviceWrapper const volatile * get_pointer<class InputDeviceWrapper const volatile >(
		class InputDeviceWrapper const volatile *value)
	{
		return value;
	}

	template <>
	Messenger const volatile * get_pointer<class Messenger const volatile >(
		class Messenger const volatile *value)
	{
		return value;
	}

	template <>
	MessageContent const volatile * get_pointer<class MessageContent const volatile >(
		class MessageContent const volatile *value)
	{
		return value;
	}

	template <>
	ParticleController const volatile * get_pointer<class ParticleController const volatile >(
		class ParticleController const volatile *value)
	{
		return value;
	}

	template <>
	ParticleEmitterController const volatile * get_pointer<class ParticleEmitterController const volatile >(
		class ParticleEmitterController const volatile *value)
	{
		return value;
	}

	template <>
	ParticleMetadata const volatile * get_pointer<class ParticleMetadata const volatile >(
		class ParticleMetadata const volatile *value)
	{
		return value;
	}


	template <>
	PhysicsConfig const volatile * get_pointer<class PhysicsConfig const volatile >(
		class PhysicsConfig const volatile *value)
	{
		return value;
	}

	template <>
	PhysicsSnapshot const volatile * get_pointer<class PhysicsSnapshot const volatile >(
		class PhysicsSnapshot const volatile *value)
	{
		return value;
	}

	template <>
	Position const volatile * get_pointer<class Position const volatile >(
		class Position const volatile *value)
	{
		return value;
	}

	template <>
	QueryManager const volatile * get_pointer<class QueryManager const volatile >(
		class QueryManager const volatile *value)
	{
		return value;
	}

	template <>
	RenderEffects const volatile * get_pointer<class RenderEffects const volatile >(
		class RenderEffects const volatile *value)
	{
		return value;
	}

	template <>
	Renderer const volatile * get_pointer<class Renderer const volatile >(
		class Renderer const volatile *value)
	{
		return value;
	}

	template <>
	RenderableText const volatile * get_pointer<class RenderableText const volatile >(
		class RenderableText const volatile *value)
	{
		return value;
	}

	template <>
	RenderableTextController const volatile * get_pointer<class RenderableTextController const volatile >(
		class RenderableTextController const volatile *value)
	{
		return value;
	}

	template <>
	Room const volatile * get_pointer<class Room const volatile >(
		class Room const volatile *value)
	{
		return value;
	}

	template <>
	RoomContainer const volatile * get_pointer<class RoomContainer const volatile >(
		class RoomContainer const volatile *value)
	{
		return value;
	}

	template <>
	RoomManager const volatile * get_pointer<class RoomManager const volatile >(
		class RoomManager const volatile *value)
	{
		return value;
	}

	template <>
	RoomMetadata const volatile * get_pointer<class RoomMetadata const volatile >(
		class RoomMetadata const volatile *value)
	{
		return value;
	}

	template <>
	Size const volatile * get_pointer<class Size const volatile >(
		class Size const volatile *value)
	{
		return value;
	}

	template <>
	StateMachineController const volatile * get_pointer<class StateMachineController const volatile >(
		class StateMachineController const volatile *value)
	{
		return value;
	}

	template <>
	SpriteSheet const volatile * get_pointer<class SpriteSheet const volatile >(
		class SpriteSheet const volatile *value)
	{
		return value;
	}

	template <>
	StateMachineState const volatile * get_pointer<class StateMachineState const volatile >(
		class StateMachineState const volatile *value)
	{
		return value;
	}
	
	template <>
	StageController const volatile * get_pointer<class StageController const volatile >(
		class StageController const volatile *value)
	{
		return value;
	}


	template <>
	StageElements const volatile * get_pointer<class StageElements const volatile >(
		class StageElements const volatile *value)
	{
		return value;
	}

	template <>
	TextManager const volatile * get_pointer<class TextManager const volatile >(
		class TextManager const volatile *value)
	{
		return value;
	}

	template <>
	TileCollisionData const volatile * get_pointer<class TileCollisionData const volatile >(
		class TileCollisionData const volatile *value)
	{
		return value;
	}

	template <>
	TransitionManager const volatile * get_pointer<class TransitionManager const volatile >(
		class TransitionManager const volatile *value)
	{
		return value;
	}

	template <>
	Ui const volatile * get_pointer<class Ui const volatile >(
		class Ui const volatile *value)
	{
		return value;
	}

	template <>
	UiPanel const volatile * get_pointer<class UiPanel const volatile >(
		class UiPanel const volatile *value)
	{
		return value;
	}

	template <>
	UiPanelElementDefinition const volatile * get_pointer<class UiPanelElementDefinition const volatile >(
		class UiPanelElementDefinition const volatile *value)
	{
		return value;
	}
	
	template <>
	UiWidget const volatile * get_pointer<class UiWidget const volatile >(
		class UiWidget const volatile *value)
	{
		return value;
	}
}


bool RoomManager::isPythonInitialized_ = false;

RoomManager::RoomManager(AnimationManagerPtr animationManager,
						 boost::shared_ptr<Assets> assets,
						 boost::shared_ptr<AudioPlayer> audioPlayer,
						 boost::shared_ptr<BaseIds> ids,
						 boost::shared_ptr<CameraManager> cameraManager,
						 boost::shared_ptr<EngineConfig> engineConfig,
						 boost::shared_ptr<EngineController> engineController,
						 boost::shared_ptr<FontManager> fontManager,
						 boost::shared_ptr<HitboxManager> hitboxManager,
						 boost::shared_ptr<InputDeviceManager> inputDeviceManager,
						 IoService ioService,
						 boost::shared_ptr<LoadingScreenContainer> loadingScreenContainer,
						 boost::shared_ptr<Messenger> messenger,
						 boost::shared_ptr<PhysicsConfig> physicsConfig,
						 boost::shared_ptr<QueryManager> queryManager,
						 boost::shared_ptr<Renderer> renderer,
						 boost::shared_ptr<RoomContainer> roomContainer,
						 boost::shared_ptr<TextManager> textManager,
						 boost::shared_ptr<GameTimer> timer,
						 boost::shared_ptr<TransitionManager> transitionManager,
						 boost::shared_ptr<Ui> ui,
						 DebuggerPtr debugger)
{
	//anchorPointManager_ = anchorPointManager;
	animationManager_ = animationManager;
	assets_ = assets;
	audioPlayer_ = audioPlayer;
	camera_ = nullptr;
	cameraManager_ = cameraManager;
	engineConfig_ = engineConfig;
	engineController_ = engineController;
	fontManager_ = fontManager;
	hitboxManager_ = hitboxManager;
	ids_ = ids;
	inputDeviceManager_ = inputDeviceManager;
	ioService_ = ioService;
	loadingScreenContainer_ = loadingScreenContainer;
	messenger_ = messenger;
	physicsConfig_ = physicsConfig;
	queryManager_ = queryManager;
	renderer_ = renderer;
	roomContainer_ = roomContainer;
	textManager_ = textManager;
	timer_ = timer;
	transitionManager_ = transitionManager;
	ui_ = ui;
	debugger_ = debugger;

	isInitialized_ = false;
	isRoomLoaded_ = false;
	
	roomToLoad_ = ids_->ROOM_NULL;
	loadedRoom_ = ids_->ROOM_NULL;
	
	if (isPythonInitialized_ == false)
	{
		PyImport_AppendInittab("firemelon", &PyInit_firemelon);

		Py_Initialize();
		PyEval_InitThreads();


		object main_module = import("__main__");
		object main_namespace = main_module.attr("__dict__");
		object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
		object pyFiremelonNamespace = pyFiremelonModule.attr("__dict__");

		// Add the scripts directory under the current working directory to the class path.
		str pyCode("import os\n"
				   "import sys\n"
				   "sys.path.append(os.getcwd() + '\\Source\\Scripts')\n");
	
		object obj = exec(pyCode, main_namespace);

		//pyFiremelonNamespace["anchorPointManager"] = ptr(anchorPointManager_.get());
		pyFiremelonNamespace["animationManager"] = ptr(animationManager_.get());
		pyFiremelonNamespace["audioPlayer"] = ptr(audioPlayer_.get());
		pyFiremelonNamespace["cameraManager"] = ptr(cameraManager_.get());
		pyFiremelonNamespace["debugger"] = ptr(debugger_.get());
		pyFiremelonNamespace["engineController"] = ptr(engineController_.get());
		pyFiremelonNamespace["fontManager"] = ptr(fontManager_.get());
		pyFiremelonNamespace["hitboxManager"] = ptr(hitboxManager_.get());
		pyFiremelonNamespace["ids"] = ptr(ids_.get());
		pyFiremelonNamespace["inputDeviceManager"] = ptr(inputDeviceManager_.get());
		pyFiremelonNamespace["messenger"] = ptr(messenger_.get());
		pyFiremelonNamespace["physics"] = ptr(physicsConfig_.get());
		pyFiremelonNamespace["queryManager"] = ptr(queryManager_.get());
		pyFiremelonNamespace["renderer"] = ptr(renderer_.get());
		pyFiremelonNamespace["roomContainer"] = ptr(roomContainer_.get());
		pyFiremelonNamespace["roomManager"] = ptr(this);
		pyFiremelonNamespace["textManager"] = ptr(textManager_.get());
		//pyFiremelonNamespace["timer"] = ptr(timer_.get());
		pyFiremelonNamespace["transitionManager"] = ptr(transitionManager_.get());
		pyFiremelonNamespace["ui"] = ptr(ui.get());

		state_ = PyEval_SaveThread();

		isPythonInitialized_ = true;
	}
}

RoomManager::~RoomManager()
{
	bool debug = true;
}

void RoomManager::shutdown()
{
	if (isInitialized_ == true)
	{
		roomContainer_->showRoomSignal_->disconnect(boost::bind(&RoomManager::showRoom, this, _1, _2, _3));
	}

	if (isPythonInitialized_ == true)
	{
		PyEval_RestoreThread(state_);
		Py_Finalize();

		isPythonInitialized_ = false;
	}
}

void RoomManager::initialize()
{	
	if (isInitialized_ == false)
	{
		renderableManager_->renderer_ = renderer_;
		renderableManager_->animationManager_ = animationManager_;
		//renderableManager_->anchorPointManager_ = anchorPointManager_;

		roomContainer_->showRoomSignal_->connect(boost::bind(&RoomManager::showRoom, this, _1, _2, _3));

		isInitialized_ = true;
	}
}

void RoomManager::setAssets(boost::shared_ptr<Assets> assets)
{	
	assets_ = assets;
}

boost::shared_ptr<RoomContainer> RoomManager::getRoomContainer()
{	
	return roomContainer_;
}

RoomId RoomManager::getShownRoomIdPy()
{
	PythonReleaseGil unlocker;

	return getShownRoomId();
}

RoomId RoomManager::getShownRoomId()
{
	return roomContainer_->getShownRoomId();
}

void RoomManager::loadRoomByNamePy(std::string roomName, stringmap roomParameters)
{
	PythonReleaseGil unlocker;
	
	loadRoomByName(roomName, roomParameters);
}

void RoomManager::loadRoomByName(std::string roomName, stringmap roomParameters)
{
	RoomId roomId = BaseIds::nameIdMap[roomName];

	loadRoom(roomId, roomParameters);
}

void RoomManager::loadRoom2(RoomId roomId)
{
	boost::shared_ptr<Room> roomToLoad = roomContainer_->getRoom(roomId);

	if (roomToLoad != nullptr)
	{
		if (roomToLoad->getIsLoaded() == false && roomToLoad->getIsLoading() == false)
		{
			if (roomToLoad->isUnloading_ == true)
			{
				std::cout << "Room unloading" << std::endl;
			}

			roomToLoad->setIsLoading(true);

			if (engineController_->getHasQuit() == false)
			{
				ioService_->post(boost::bind(&Room::loadRoomAsync, roomToLoad));
			}
			else
			{
				std::cout << "Failed to load room because the engine is shutting down." << std::endl;
			}
		}
	}
	else
	{
		std::cout<<"Error: Unable to load room \""<<roomId<<"\". Room ID not found."<<std::endl;
	}
	
	return;
}

void RoomManager::showRoomByNamePy(std::string roomName, TransitionId transitionId, double transitionTime)
{
	PythonReleaseGil unlocker;

	showRoomByName(roomName, transitionId, transitionTime);
}

void RoomManager::showRoomByName(std::string roomName, TransitionId transitionId, double transitionTime)
{
	RoomId roomId = BaseIds::nameIdMap[roomName];

	showRoom(roomId, transitionId, transitionTime);
}

void RoomManager::showRoomPy(RoomId roomId, TransitionId transitionId, double transitionTime)
{
	PythonReleaseGil unlocker;

	showRoom(roomId, transitionId, transitionTime);
}

void RoomManager::showRoom(RoomId roomId, TransitionId transitionId, double transitionTime)
{
	if (getShownRoomId() != roomId)
	{
		if (transitionId == ids_->TRANSITION_NULL)
		{
			showRoomImmediate(roomId);
		}
		else
		{
			// If the room is not loaded or loading yet, load it now.
			boost::shared_ptr<Room> roomToShow = roomContainer_->getRoom(roomId);

			if (roomToShow != nullptr)
			{
				if (roomToShow->getIsLoaded() == false)
				{
					// Not loaded yet. If it is not loading, load it.
					if (roomToShow->getIsLoading() == false)
					{
						loadRoom2(roomId);
					}
				}

				if (transitionManager_ != nullptr)
				{
					if (transitionManager_->getIsTransitioning() == false)
					{
						// Queue up the room to be shown, after the transition completes.
						transitionManager_->activateRoomChangeTransition(transitionId, transitionTime, roomId);
					}
				}
			}
		}
	}

	return;
}

void RoomManager::showRoomImmediate(RoomId roomId)
{
	RoomId previousShowingRoomId = roomContainer_->getShownRoomId();
	boost::shared_ptr<Room> previousShowingRoom = roomContainer_->getRoom(previousShowingRoomId);
	
	if (previousShowingRoom != nullptr)
	{
		previousShowingRoom->preRoomHidden();
	}

	boost::shared_ptr<Room> roomToShow = roomContainer_->getRoom(roomId);

	if (roomToShow != nullptr)
	{
		if (roomToShow->getIsLoaded() == false)
		{
			// Not loaded yet. If it is not loading, load it.
			if (roomToShow->getIsLoading() == false)
			{
				loadRoom2(roomId);
			}
		}
		
		int loadingScreenId = roomToShow->getLoadingScreenId();

		boost::shared_ptr<LoadingScreen> loadingScreen = nullptr;

		if (loadingScreenId > -1)
		{
			loadingScreen = loadingScreenContainer_->getLoadingScreen(loadingScreenId);
		}

		int percentLoaded = 0;
		int newPercentLoaded = 0;

		// At this point the room will either be loaded or be loading. If it is not yet loaded wait until it is.
		while (roomToShow->getIsLoaded() == false)
		{
			// Show the room loader associated with this room if one exists.
			newPercentLoaded = roomToShow->getPercentLoaded();

			if (newPercentLoaded > percentLoaded)
			{
				if (loadingScreen != nullptr)
				{
					loadingScreen->percentChanged(newPercentLoaded);
					
					renderer_->sceneComplete();
					renderer_->sceneBegin();
				}
				else
				{
					std::cout<<"Waiting for room to finish loading... "<<newPercentLoaded<<"%"<<std::endl;
				}

				percentLoaded = newPercentLoaded;
			}
		}
		
		roomContainer_->setShownRoomId(roomId);
		
		// There may to be a large delta time after waiting for the room to load.
		timer_->tick();

		roomToShow->preRoomShown();
	}
	else
	{
		std::cout<<"Error: Unable to show room \""<<roomId<<"\". Room ID not found."<<std::endl;
	}
}

void RoomManager::moveAllEntities()
{
	roomContainer_->moveAllEntities();
}

void RoomManager::loadRoomPy(RoomId roomId, stringmap roomParameters)
{
	PythonReleaseGil unlocker;

	loadRoom(roomId, roomParameters);
}

void RoomManager::loadRoom(RoomId roomId, stringmap roomParameters)
{
	// If nothing has been loaded yet, the room can be loaded immediately.
	// Otherwise, it will have to be queued up to load after all updates are completed.
	
	roomToLoad_ = roomId;	

	roomParameters_ = roomParameters;

	if (isRoomLoaded_ == false)
	{		
		loadQueuedRoom();
	}
}

void RoomManager::unloadRoomPy(RoomId roomId)
{
	PythonReleaseGil unlocker;

	unloadRoom(roomId);
}

void RoomManager::unloadRoom(RoomId roomId)
{
	boost::shared_ptr<Room> roomToUnload = roomContainer_->getRoom(roomId);

	if (roomToUnload != nullptr)
	{
		if (roomToUnload->getIsLoaded() == true && roomToUnload->getIsLoading() == false)
		{
			if (engineController_->getHasQuit() == false)
			{
				ioService_->post(boost::bind(&Room::unloadRoomAsync, roomToUnload));
			}
			else
			{
				std::cout << "Failed to load room because the engine is shutting down." << std::endl;
			}
		}
	}
	else
	{
		std::cout << "Error: Unable to unload room \"" << roomId << "\". Room ID not found." << std::endl;
	}

	return;
}

void RoomManager::loadQueuedRoom()
{
	return;
}

void RoomManager::loadAssets()
{
	if (debugger_->debugLevel >= 1)
	{
		std::cout << "Loading assets..." << std::endl;
	}

	assets_->load();

	if (debugger_->debugLevel >= 1)
	{
		std::cout << "Loading rooms" << std::endl;
	}
	
	renderableManager_->screenWidth_ = assets_->getCameraWidth();
	renderableManager_->screenHeight_ = assets_->getCameraHeight();

	roomContainer_->preloadRooms();
	
	PythonAcquireGil lock;

	// Write out the Names and IDs, for debugging purposes.	
	int size = BaseIds::idNames.size();
	for (int i = 0; i < size; i++)
	{
		std::string name = BaseIds::idNames[i].getName();
		int id = BaseIds::idNames[i].getId();
		std::cout<<name<<" = "<<id<<std::endl;

		BaseIds::nameIdMap[name] = id;
		BaseIds::idNameMap[id] = name;

		try
		{
			// Add the name-ID variables to the firemelon namespace.
			object pyMainModule_ = import("__main__");
			object pyMainNamespace_ = pyMainModule_.attr("__dict__");	
			object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
			object pyFiremelonNamespace = pyFiremelonModule.attr("__dict__");	

			std::string sCode = name + " = " + boost::lexical_cast<std::string>(id);

			str pyCode(sCode);	
			boost::python::object obj = boost::python::exec(pyCode, pyFiremelonNamespace);
		}
		catch(error_already_set &)
		{
			debugger_->handlePythonError();
		}
	}

	try
	{
		// Add the name-ID variables to the firemelon namespace.
		object pyMainModule_ = import("__main__");
		object pyMainNamespace_ = pyMainModule_.attr("__dict__");	
		object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
		object pyFiremelonNamespace = pyFiremelonModule.attr("__dict__");	
		
		pyFiremelonNamespace["entityMetadataContainer"] = ptr(entityMetadataContainer_.get());		
		pyFiremelonNamespace["queryManager"] = ptr(queryManager_.get());
	}
	catch(error_already_set &)
	{
		debugger_->handlePythonError();
	}
}
