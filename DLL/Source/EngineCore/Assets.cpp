#include "..\..\Headers\EngineCore\Assets.hpp"

using namespace firemelon;
using namespace boost::python;

Assets::Assets(boost::shared_ptr<RenderableManager> renderableManager,
			   boost::shared_ptr<HitboxManager> hitboxManager,
			   AnimationManagerPtr animationManager,
			   boost::shared_ptr<Ui> ui,
			   boost::shared_ptr<InputDeviceManager> inputDeviceManager,
			   boost::shared_ptr<Renderer> renderer,
			   boost::shared_ptr<AudioPlayer> audioPlayer,
			   boost::shared_ptr<EntityMetadataContainer> entityMetadataContainer,
			   boost::shared_ptr<AnchorPointManager> anchorPointManager,
			   boost::shared_ptr<LoadingScreenContainer> loadingScreenContainer,
			   boost::shared_ptr<TransitionContainer> transitionContainer,
			   boost::shared_ptr<TextManager> textManager,
			   boost::shared_ptr<FontManager> fontManager,
			   boost::shared_ptr<BaseIds> ids,
			   boost::shared_ptr<Debugger> debugger)
{
	tileSize_ = 0;
	cameraHeight_ = 0;
	cameraWidth_ = 0;
	tileWidthPerRenderGridCell_ = 6;
	tileWidthPerCollisionGridCell_ = 3;
	
	debugger_ = debugger;
	renderableManager_ = renderableManager;
	hitboxManager_ = hitboxManager;
	animationManager_ = animationManager;
	ui_ = ui;
	inputDeviceManager_ = inputDeviceManager;
	renderer_ = renderer;
	audioPlayer_ = audioPlayer;
	entityMetadataContainer_ = entityMetadataContainer;
	anchorPointManager_ = anchorPointManager;
	loadingScreenContainer_ = loadingScreenContainer;
	transitionContainer_ = transitionContainer;
	textManager_ = textManager;
	fontManager_ = fontManager;
	ids_ = ids;
}

Assets::~Assets()
{
	entityTemplates_.clear();	
}

RoomId Assets::getInitialRoomId()
{
	return initialRoomId_;
}

int Assets::getTileSize()
{
	return tileSize_;
}

int Assets::getTileWidthPerRenderGridCell()
{
	return tileWidthPerRenderGridCell_;
}

int Assets::getTileWidthPerCollisionGridCell()
{
	return tileWidthPerCollisionGridCell_;
}

int Assets::getCameraWidth()
{
	return cameraWidth_;
}

int	Assets::getCameraHeight()
{
	return cameraHeight_;
}

boost::shared_ptr<EntityTemplate> Assets::getEntityTemplate(EntityTypeId entityId)
{
	return entityTemplates_[entityTemplateIdMap_[entityId]];
}

void Assets::load()
{
	boost::shared_ptr<Animation> newAnimation;
	boost::shared_ptr<AnimationFrame> newFrame;
	StageElementsPtr newStageElements;
	boost::shared_ptr<EntityTemplate> newEntityTemplate;
	
	// Start by reading the header file.
	bool isAnimated = false;
	int tileFrames = 0;
	
	int uuidSize = boost::uuids::uuid::static_size();

	// Open the file for reading.
	std::ifstream engineInitFile;
	
	std::string filename = "Data\\engine.init";

	engineInitFile.open(filename.c_str(), std::ios::in | std::ios::binary);

	if (engineInitFile.is_open())
	{
		if (debugger_->debugLevel >= 1)
		{
			std::cout << "Loading initialization data..." << std::endl;		
		}

		int stringSize = 0;

		int fileMajorVersion = 0;
		engineInitFile.read((char*)&fileMajorVersion, sizeof(int));

		int fileMinorVersion = 0;
		engineInitFile.read((char*)&fileMinorVersion, sizeof(int));
		
		int fileRevisionVersion = 0;
		engineInitFile.read((char*)&fileRevisionVersion, sizeof(int));

		engineInitFile.read((char*)&cameraHeight_, sizeof(int));
		engineInitFile.read((char*)&cameraWidth_, sizeof(int));

		engineInitFile.read((char*)&tileSize_, sizeof(int));

		renderableManager_->setTileSize(tileSize_);
		renderableManager_->setGridCellSize((tileSize_ * tileWidthPerRenderGridCell_));
		
		engineInitFile.close();
	}
	
	renderer_->setScreenSize(cameraWidth_, cameraHeight_);

	renderer_->initializeScreen();

	// Load the tile sheet files.
	boost::filesystem::path assets(".\\Data\\Assets\\");
	boost::filesystem::directory_iterator end_itr;

	for (boost::filesystem::directory_iterator itr(assets); itr != end_itr; ++itr)
    {
		boost::filesystem::path filepath = itr->path();
		boost::filesystem::path extension = itr->path().extension();

        if (extension == ".ta") 
		{
			if (debugger_->debugLevel >= 1)
			{
				std::cout << "Loading tile sheet " << filepath << "..." << std::endl;
			}

			std::ifstream tileSheetFile;
	
			tileSheetFile.open(itr->path().c_str(), std::ios::in | std::ios::binary);

			if (tileSheetFile.is_open())
			{
				int fileMajorVersion = 0;
				tileSheetFile.read((char*)&fileMajorVersion, sizeof(int));

				int fileMinorVersion = 0;
				tileSheetFile.read((char*)&fileMinorVersion, sizeof(int));
		
				int fileRevisionVersion = 0;
				tileSheetFile.read((char*)&fileRevisionVersion, sizeof(int));

				int tileSheetNameSize = 0;
				tileSheetFile.read((char*)&tileSheetNameSize, sizeof(int));

				std::string tileSheetName(tileSheetNameSize, '\0');
				tileSheetFile.read(&tileSheetName[0], tileSheetNameSize);
			
				boost::uuids::uuid tileSheetUuid;
				char* uuidBytes = new char[uuidSize];
				tileSheetFile.read(uuidBytes, uuidSize);		
				memcpy(&tileSheetUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				AssetId tileSheetId = BaseIds::getIntegerFromUuid(tileSheetUuid);
				/*
				NameIdPair nameId(tileSheetId, pythonVariableName);

				BaseIds::idNames.push_back(nameId);
*/
				boost::uuids::uuid bitmapUuid;
				uuidBytes = new char[uuidSize];
				tileSheetFile.read(uuidBytes, uuidSize);		
				memcpy(&bitmapUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				int tileSheetRows = 0;
				tileSheetFile.read((char*)&tileSheetRows, sizeof(int));

				int tileSheetCols = 0;
				tileSheetFile.read((char*)&tileSheetCols, sizeof(int));

				int tileWidth = 0;
				tileSheetFile.read((char*)&tileWidth, sizeof(int));

				int tileHeight = 0;
				tileSheetFile.read((char*)&tileHeight, sizeof(int));

				float scaleFactor = 1;
				tileSheetFile.read((char*)&scaleFactor, sizeof(float));

				// Read the bitmap file to a byte array.
				std::string bitmapFileName = ".\\Data\\Assets\\" + boost::lexical_cast<std::string>(bitmapUuid) + ".br";
				
				std::ifstream bitmapFile;
	
				bitmapFile.open(bitmapFileName.c_str(), std::ios::in | std::ios::binary);
				
				int tileSurfaceId = 0;

				if (bitmapFile.is_open())
				{
					int fileMajorVersion = 0;
					bitmapFile.read((char*)&fileMajorVersion, sizeof(int));

					int fileMinorVersion = 0;
					bitmapFile.read((char*)&fileMinorVersion, sizeof(int));
		
					int fileRevisionVersion = 0;
					bitmapFile.read((char*)&fileRevisionVersion, sizeof(int));

					// Don't actually need this ID here. Just ignore it
					boost::uuids::uuid bitmapUuid2;
					uuidBytes = new char[uuidSize];
					bitmapFile.read(uuidBytes, uuidSize);		
					memcpy(&bitmapUuid2, uuidBytes, uuidSize);
					delete uuidBytes;

					int imageSize = 0;
					bitmapFile.read((char*)&imageSize, sizeof(int));

					// Load the image file into a temporary buffer, then load it into the renderer as a surface.
					char* imageBuffer = new char[imageSize];
					bitmapFile.read((char*)imageBuffer, imageSize);
		
					// Need to add scale factor to tile sheet export.
					tileSurfaceId = renderer_->loadSpriteSheet(tileSheetName, imageBuffer, imageSize, tileSheetRows, tileSheetCols, tileHeight, tileWidth, scaleFactor, true, 0);
					
					// Map the new ID to the old.
					renderer_->spritesheetIdMap_[tileSheetId] = tileSurfaceId;

					delete[] imageBuffer;

					bitmapFile.close();
				}

				// Create two sprite entityTemplate for tiles. One with a hitbox and one without.
			
				// First create one with the hitbox.
				newEntityTemplate = boost::shared_ptr<EntityTemplate>(new EntityTemplate());

				newEntityTemplate->renderer_ = renderer_;

				// Create the hitboxes, one for each tile type.

				//CollisionMaskType maskTypes[] = {
				//	SOLID_MASK,
				//	SLOPE_TRANSITON_MASK,
				//	ONEWAY_TOP_MASK,
				//	SLOPE45_BOTTOMLEFT_MASK,
				//	SLOPE45_TOPLEFT_MASK,
				//	SLOPE45_BOTTOMRIGHT_MASK,
				//	SLOPE45_TOPRIGHT_MASK,
				//	SLOPE26_HORIZONTAL_BOTTOMLEFT_SMALL_MASK,
				//	SLOPE26_HORIZONTAL_TOPLEFT_SMALL_MASK,
				//	SLOPE26_HORIZONTAL_BOTTOMRIGHT_SMALL_MASK,
				//	SLOPE26_HORIZONTAL_TOPRIGHT_SMALL_MASK,
				//	SLOPE26_HORIZONTAL_BOTTOMLEFT_LARGE_MASK,
				//	SLOPE26_HORIZONTAL_TOPLEFT_LARGE_MASK,
				//	SLOPE26_HORIZONTAL_BOTTOMRIGHT_LARGE_MASK,
				//	SLOPE26_HORIZONTAL_TOPRIGHT_LARGE_MASK,
				//	SLOPE26_VERTICAL_BOTTOMLEFT_SMALL_MASK,
				//	SLOPE26_VERTICAL_TOPLEFT_SMALL_MASK,
				//	SLOPE26_VERTICAL_BOTTOMRIGHT_SMALL_MASK,
				//	SLOPE26_VERTICAL_TOPRIGHT_SMALL_MASK,
				//	SLOPE26_VERTICAL_BOTTOMLEFT_LARGE_MASK,
				//	SLOPE26_VERTICAL_TOPLEFT_LARGE_MASK,
				//	SLOPE26_VERTICAL_BOTTOMRIGHT_LARGE_MASK,
				//	SLOPE26_VERTICAL_TOPRIGHT_LARGE_MASK
				//};

				/*int tileMaskCount = sizeof(maskTypes) / sizeof(int);

				for (int i = 0; i < tileMaskCount; i++)
				{
					CollisionMaskType maskType = maskTypes[i];

					int x = 0;
					int y = 0;
					int width = tileSize_;
					int height = tileSize_;

					if (maskType != SOLID_MASK && maskType != ONEWAY_TOP_MASK)
					{
						y = -height;
						height *= 2;
					}

					boost::shared_ptr<Hitbox> tileHitbox = boost::shared_ptr<Hitbox>(new Hitbox(x, y, height, width));
					tileHitbox->setIdentity(ids_->HITBOX_TILE_MAIN);

					tileHitbox->setIgnoreContiguousTileEdges(false);
					
					tileHitbox->setCollisionMaskType(maskType);

					int hitboxId = hitboxManager_->addHitbox(tileHitbox);

					hitboxCollisionMaskIdMap_[maskType] = hitboxId;
				}*/

				tileSheetFile.close();
			}
			
		}
	}

	// Load the sprite sheet files.	
	for (boost::filesystem::directory_iterator itr(assets); itr != end_itr; ++itr)
    {
		boost::filesystem::path filepath = itr->path();
		boost::filesystem::path extension = itr->path().extension();

        if (extension == ".sa") 
		{
			if (debugger_->debugLevel >= 1)
			{
				std::cout << "Loading sprite sheet " << filepath << "..." << std::endl;
			}

			std::ifstream spriteSheetFile;
	
			spriteSheetFile.open(itr->path().c_str(), std::ios::in | std::ios::binary);

			if (spriteSheetFile.is_open())
			{
				int sheetNameSize = 0;
				int newSheetId = 0;
				int sheetCellHeight = 0;
				int sheetCellWidth = 0;
				int sheetRows = 0;
				int sheetCols = 0;
				float scaleFactor = 1;
				int padding = 0;
				
				int fileMajorVersion = 0;
				spriteSheetFile.read((char*)&fileMajorVersion, sizeof(int));

				int fileMinorVersion = 0;
				spriteSheetFile.read((char*)&fileMinorVersion, sizeof(int));
		
				int fileRevisionVersion = 0;
				spriteSheetFile.read((char*)&fileRevisionVersion, sizeof(int));

				spriteSheetFile.read((char*)&sheetNameSize, sizeof(int));

				std::string sheetName(sheetNameSize, '\0');
				spriteSheetFile.read(&sheetName[0], sheetNameSize);
			
				boost::uuids::uuid spriteSheetUuid;
				char* uuidBytes = new char[uuidSize];
				spriteSheetFile.read(uuidBytes, uuidSize);		
				memcpy(&spriteSheetUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				AssetId spriteSheetId = BaseIds::getIntegerFromUuid(spriteSheetUuid);

				boost::uuids::uuid bitmapUuid;
				uuidBytes = new char[uuidSize];
				spriteSheetFile.read(uuidBytes, uuidSize);		
				memcpy(&bitmapUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				spriteSheetFile.read((char*)&sheetCellHeight, sizeof(int));
				spriteSheetFile.read((char*)&sheetCellWidth, sizeof(int));
				spriteSheetFile.read((char*)&sheetRows, sizeof(int));
				spriteSheetFile.read((char*)&sheetCols, sizeof(int));
				spriteSheetFile.read((char*)&scaleFactor, sizeof(float));
				spriteSheetFile.read((char*)&padding, sizeof(int));

				// Read the bitmap file to a byte array.
				std::string bitmapFileName = ".\\Data\\Assets\\" + boost::lexical_cast<std::string>(bitmapUuid) + ".br";

				std::ifstream bitmapFile;
	
				bitmapFile.open(bitmapFileName.c_str(), std::ios::in | std::ios::binary);
				
				if (bitmapFile.is_open())
				{
					int fileMajorVersion = 0;
					bitmapFile.read((char*)&fileMajorVersion, sizeof(int));

					int fileMinorVersion = 0;
					bitmapFile.read((char*)&fileMinorVersion, sizeof(int));
		
					int fileRevisionVersion = 0;
					bitmapFile.read((char*)&fileRevisionVersion, sizeof(int));

					// Don't actually need this ID here. Just ignore it
					boost::uuids::uuid bitmapUuid2;
					uuidBytes = new char[uuidSize];
					bitmapFile.read(uuidBytes, uuidSize);		
					memcpy(&bitmapUuid2, uuidBytes, uuidSize);
					delete uuidBytes;

					int imageSize = 0;
					bitmapFile.read((char*)&imageSize, sizeof(int));
			
					char* imageBuffer = new char[imageSize];

					bitmapFile.read((char*)imageBuffer, imageSize);
			
					newSheetId = renderer_->loadSpriteSheet(sheetName, imageBuffer, imageSize, sheetRows, sheetCols, sheetCellHeight, sheetCellWidth, scaleFactor, true, padding);

					renderer_->spritesheetIdMap_[spriteSheetId] = newSheetId;

					delete[] imageBuffer;

					bitmapFile.close();
				}

				spriteSheetFile.close();
			}
		}
	}

	renderer_->initializeShader();

	renderer_->sheetsLoaded();

	// Load the engine meta data.
	
	// Open the file for reading.
	std::ifstream metadataFile;
	
	filename = "Data\\meta.data";

	metadataFile.open(filename.c_str(), std::ios::in | std::ios::binary);

	if (metadataFile.is_open())
	{
		if (debugger_->debugLevel >= 1)
		{
			std::cout << "Loading metadata..." << std::endl;
		}

		int stringSize = 0;

		int fileMajorVersion = 0;
		metadataFile.read((char*)&fileMajorVersion, sizeof(int));

		int fileMinorVersion = 0;
		metadataFile.read((char*)&fileMinorVersion, sizeof(int));
		
		int fileRevisionVersion = 0;
		metadataFile.read((char*)&fileRevisionVersion, sizeof(int));

		boost::uuids::uuid initialRoomUuid;
		char* uuidBytes = new char[uuidSize];
		metadataFile.read((char*)uuidBytes, uuidSize);		
		memcpy(&initialRoomUuid, uuidBytes, uuidSize);
		delete uuidBytes;

		std::string roomId = boost::lexical_cast<std::string>(initialRoomUuid);

		initialRoomId_ = BaseIds::getIntegerFromUuid(initialRoomUuid);
		
		// Getting rid of room loader soon...
		//roomLoader_->scriptTypeName_ = roomLoaderTypeName;
		//roomLoader_->scriptName_ = roomLoaderScriptName;

		int uiScriptNameSize = 0;

		metadataFile.read((char*)&uiScriptNameSize, sizeof(int));

		std::string uiScriptName(uiScriptNameSize, '\0');
		metadataFile.read(&uiScriptName[0], uiScriptNameSize);
			
		int uiTypeNameSize = 0;

		metadataFile.read((char*)&uiTypeNameSize, sizeof(int));

		std::string uiTypeName(uiTypeNameSize, '\0');
		metadataFile.read(&uiTypeName[0], uiTypeNameSize);
			
		ui_->scriptTypeName_ = uiTypeName;
		ui_->scriptName_ = uiScriptName;
		
		
		int networkHandlerScriptNameSize = 0;

		metadataFile.read((char*)&networkHandlerScriptNameSize, sizeof(int));

		std::string networkHandlerScriptName(networkHandlerScriptNameSize, '\0');
		metadataFile.read(&networkHandlerScriptName[0], networkHandlerScriptNameSize);
			
		int networkHandlerTypeNameSize = 0;

		metadataFile.read((char*)&networkHandlerTypeNameSize, sizeof(int));

		std::string networkHandlerTypeName(networkHandlerTypeNameSize, '\0');
		metadataFile.read(&networkHandlerTypeName[0], networkHandlerTypeNameSize);
		
		int hitboxIdentityCount = 0;
		metadataFile.read((char*)&hitboxIdentityCount, sizeof(int));
			
		for (int j = 0; j < hitboxIdentityCount; j++)
		{
			int hitboxIdentityNameSize = 0;

			metadataFile.read((char*)&hitboxIdentityNameSize, sizeof(int));

			std::string hitboxIdentityName(hitboxIdentityNameSize, '\0');
			metadataFile.read(&hitboxIdentityName[0], hitboxIdentityNameSize);
			
			boost::uuids::uuid hitboxIdentityUuid;
			char* uuidBytes = new char[uuidSize];
			metadataFile.read(uuidBytes, uuidSize);		
			memcpy(&hitboxIdentityUuid, uuidBytes, uuidSize);
			delete uuidBytes;

			HitboxIdentity hitboxIdentityId = BaseIds::getIntegerFromUuid(hitboxIdentityUuid);
							
			NameIdPair nameId(hitboxIdentityId, hitboxIdentityName);

			BaseIds::idNames.push_back(nameId);
		}

		int triggerSignalCount = 0;
		metadataFile.read((char*)&triggerSignalCount, sizeof(int));

		for (int j = 0; j < triggerSignalCount; j++)
		{
			int triggerSignalNameSize = 0;

			metadataFile.read((char*)&triggerSignalNameSize, sizeof(int));

			std::string triggerSignalName(triggerSignalNameSize, '\0');
			metadataFile.read(&triggerSignalName[0], triggerSignalNameSize);

			boost::uuids::uuid triggerSignalUuid;
			char* uuidBytes = new char[uuidSize];
			metadataFile.read(uuidBytes, uuidSize);
			memcpy(&triggerSignalUuid, uuidBytes, uuidSize);
			delete uuidBytes;

			TriggerSignalId triggerSignalId = BaseIds::getIntegerFromUuid(triggerSignalUuid);

			NameIdPair nameId(triggerSignalId, triggerSignalName);

			BaseIds::idNames.push_back(nameId);
		}

		int animationCount = 0;
		metadataFile.read((char*)&animationCount, sizeof(int));
			
		for (int j = 0; j < animationCount; j++)
		{
			newAnimation = boost::shared_ptr<Animation>(new Animation());

			int animationNameSize = 0;

			metadataFile.read((char*)&animationNameSize, sizeof(int));

			std::string animationName(animationNameSize, '\0');
			metadataFile.read(&animationName[0], animationNameSize);
			
			boost::uuids::uuid animationUuid;
			char* uuidBytes = new char[uuidSize];
			metadataFile.read(uuidBytes, uuidSize);		
			memcpy(&animationUuid, uuidBytes, uuidSize);
			delete uuidBytes;

			AssetId animationId = BaseIds::getIntegerFromUuid(animationUuid);

			boost::uuids::uuid spriteSheetUuid;
			uuidBytes = new char[uuidSize];
			metadataFile.read(uuidBytes, uuidSize);		
			memcpy(&spriteSheetUuid, uuidBytes, uuidSize);
			delete uuidBytes;
					
			AssetId spriteSheetId = BaseIds::getIntegerFromUuid(spriteSheetUuid);


			boost::uuids::uuid alphaMaskSheetUuid;
			uuidBytes = new char[uuidSize];
			metadataFile.read(uuidBytes, uuidSize);
			memcpy(&alphaMaskSheetUuid, uuidBytes, uuidSize);
			delete uuidBytes;

			AssetId alphaMaskSheetId = -1;
			
			if (alphaMaskSheetUuid != boost::uuids::nil_uuid())
			{
				alphaMaskSheetId = BaseIds::getIntegerFromUuid(alphaMaskSheetUuid);
			}

			// Get the new spritesheet ID that is linked to the old ID.
			int spriteSheetIndex = renderer_->spritesheetIdMap_[spriteSheetId];
			
			auto spriteSheet = renderer_->getSheet(spriteSheetIndex);

			int sheetCols = spriteSheet->getColumns();

			// Get the new spritesheet ID that is linked to the old ID.
			int alphaMaskSheetIndex = renderer_->spritesheetIdMap_[alphaMaskSheetId];

			int alphaMaskSheetCols = renderer_->getSheet(alphaMaskSheetIndex)->getColumns();

			newAnimation->setName(animationName);
			newAnimation->setSpriteSheetId(spriteSheetIndex);
			newAnimation->setAlphaMaskSheetId(alphaMaskSheetIndex);

			int frameCount = 0;
			metadataFile.read((char*)&frameCount, sizeof(int));

			if (sheetCols == 0)
			{
				std::cout << "Warning: Sheet columns set to 0 in sprite sheet " << spriteSheet->getSheetName() << " for animation " << animationName <<". This will likely result in broken animations" << std::endl;
			}

			for (int k = 0; k < frameCount; k++)
			{
				int cellId = 0;
				metadataFile.read((char*)&cellId, sizeof(int));

				int cellX = 0;
				int cellY =0;

				if (sheetCols > 0)
				{
					cellX = cellId % sheetCols;
					cellY = cellId / sheetCols;
				}

				int alphaMaskCellId = 0;
				metadataFile.read((char*)&alphaMaskCellId, sizeof(int));

				int alphaMaskCellColumn = -1;
				
				int alphaMaskCellRow = -1; 

				if (alphaMaskSheetId != -1 && alphaMaskCellId != -1)
				{
					alphaMaskCellColumn = alphaMaskCellId % alphaMaskSheetCols;

					alphaMaskCellRow = alphaMaskCellId / alphaMaskSheetCols;
				}

				newFrame = boost::shared_ptr<AnimationFrame>(new AnimationFrame(cellX, cellY));

				newFrame->setAlphaMaskSheetCellColumn(alphaMaskCellColumn);

				newFrame->setAlphaMaskSheetCellRow(alphaMaskCellRow);

				newFrame->anchorPointManager_ = anchorPointManager_;

				int hitboxCount = 0;
				metadataFile.read((char*)&hitboxCount, sizeof(int));
					
				for (int l = 0; l < hitboxCount; l++)
				{
					int top = 0;
					int left = 0;
					int height = 0;
					int width = 0;

					metadataFile.read((char*)&top, sizeof(int));
					metadataFile.read((char*)&left, sizeof(int));
					metadataFile.read((char*)&height, sizeof(int));
					metadataFile.read((char*)&width, sizeof(int));
							
					boost::uuids::uuid hitboxIdentityUuid;
					char* uuidBytes = new char[uuidSize];
					metadataFile.read(uuidBytes, uuidSize);		
					memcpy(&hitboxIdentityUuid, uuidBytes, uuidSize);
					delete uuidBytes;

					unsigned int priority = 0;
					metadataFile.read((char*)&priority, sizeof(unsigned int));

					bool isSolid = 0;
					metadataFile.read((char*)&isSolid, sizeof(bool));

					float rotationDegrees = 0;
					metadataFile.read((char*)&rotationDegrees, sizeof(float));

					if (hitboxIdentityUuid != boost::uuids::nil_uuid())
					{
						HitboxIdentity hitboxIdentityId = BaseIds::getIntegerFromUuid(hitboxIdentityUuid);
								
						boost::shared_ptr<Hitbox> hitbox = boost::shared_ptr<Hitbox>(new Hitbox(left, top, height, width));
							
						hitbox->setIdentity((HitboxIdentity)hitboxIdentityId);
						
						hitbox->setIsSolid(isSolid);

						hitbox->setBaseRotationDegrees(rotationDegrees);

						int hitboxId = hitboxManager_->addHitbox(hitbox);
									
						// Add this hitbox's ID/index to the frame's list of hitboxes it points to.
						newFrame->addHitboxReference(hitboxId);
					}
				}
						
				int anchorPointCount = 0;
				metadataFile.read((char*)&anchorPointCount, sizeof(int));

				for (int l = 0; l < anchorPointCount; l++)
				{
					int anchorPointNameSize = 0;

					metadataFile.read((char*)&anchorPointNameSize, sizeof(int));

					std::string anchorPointName(anchorPointNameSize, '\0');
					metadataFile.read(&anchorPointName[0], anchorPointNameSize);
							
					int anchorPointLeft = 0;
					metadataFile.read((char*)&anchorPointLeft, sizeof(int));

					int anchorPointTop = 0;
					metadataFile.read((char*)&anchorPointTop, sizeof(int));

					AnchorPointPtr ap = AnchorPointPtr(new AnchorPoint(anchorPointName, anchorPointLeft, anchorPointTop));

					int anchorPointId = anchorPointManager_->addAnchorPoint(ap);
							
					newFrame->addAnchorPointReference(anchorPointId);
				}

				int frameTriggerCount = 0;
				metadataFile.read((char*)&frameTriggerCount, sizeof(int));

				for (int l = 0; l < frameTriggerCount; l++)
				{
					boost::uuids::uuid triggerSignalUuid;
					char* uuidBytes = new char[uuidSize];
					metadataFile.read(uuidBytes, uuidSize);
					memcpy(&triggerSignalUuid, uuidBytes, uuidSize);
					delete uuidBytes;

					TriggerSignalId triggerSignalId = BaseIds::getIntegerFromUuid(triggerSignalUuid);

					newFrame->addTriggerSignal(triggerSignalId);
				}

				newAnimation->addFrame(newFrame);
			}

			int newAnimationId = animationManager_->addAnimation(newAnimation);
				
			// Link the old ID to the new one.
			animationManager_->animationIdMap_[animationId] = newAnimationId;
		}

		// Load the audio resources.
		int audioCount = 0;

		metadataFile.read((char*)&audioCount, sizeof(int));

		for (int i = 0; i < audioCount; i++)
		{
			int audioScriptNameSize = 0;

			metadataFile.read((char*)&audioScriptNameSize, sizeof(int));

			std::string audioScriptName(audioScriptNameSize, '\0');
			metadataFile.read(&audioScriptName[0], audioScriptNameSize);

			int audioNameSize = 0;

			metadataFile.read((char*)&audioNameSize, sizeof(int));

			std::string audioName(audioNameSize, '\0');
			metadataFile.read(&audioName[0], audioNameSize);

			boost::uuids::uuid audioUuid;
			char* uuidBytes = new char[uuidSize];
			metadataFile.read(uuidBytes, uuidSize);
			memcpy(&audioUuid, uuidBytes, uuidSize);
			delete uuidBytes;

			int audioId = BaseIds::getIntegerFromUuid(audioUuid);

			NameIdPair nameId(audioId, audioScriptName);

			BaseIds::idNames.push_back(nameId);
		}

		// Load the game buttons.
		int buttonCount = 0;
		
		boost::shared_ptr<GameButtonManager> buttonManager = inputDeviceManager_->getGameButtonManager();

		metadataFile.read((char*)&buttonCount, sizeof(int));
		
		for (int i = 0; i < buttonCount; i++)
		{
			int buttonScriptNameSize = 0;

			metadataFile.read((char*)&buttonScriptNameSize, sizeof(int));

			std::string buttonScriptName(buttonScriptNameSize, '\0');
			metadataFile.read(&buttonScriptName[0], buttonScriptNameSize);
			
			int buttonNameSize = 0;

			metadataFile.read((char*)&buttonNameSize, sizeof(int));

			std::string buttonName(buttonNameSize, '\0');
			metadataFile.read(&buttonName[0], buttonNameSize);

			int buttonLabelSize = 0;

			metadataFile.read((char*)&buttonLabelSize, sizeof(int));

			std::string buttonLabel(buttonLabelSize, '\0');
			metadataFile.read(&buttonLabel[0], buttonLabelSize);

			GameButtonUuid buttonUuid;
			char* uuidBytes = new char[uuidSize];
			metadataFile.read(uuidBytes, uuidSize);		
			memcpy(&buttonUuid, uuidBytes, uuidSize);
			delete uuidBytes;

			int buttonId = BaseIds::getIntegerFromUuid(buttonUuid);

			NameIdPair nameId(buttonId, buttonScriptName);

			BaseIds::idNames.push_back(nameId);

			GameButtonGroupUuid buttonGroupUuid;
			uuidBytes = new char[uuidSize];
			metadataFile.read(uuidBytes, uuidSize);
			memcpy(&buttonGroupUuid, uuidBytes, uuidSize);
			delete uuidBytes;

			int buttonGroupId = BaseIds::getIntegerFromUuid(buttonGroupUuid);

			buttonManager->addButtonNameIdMapping(buttonId, buttonUuid, buttonGroupId, buttonName, buttonLabel);
		}


		// Load the game button groups.
		int buttonGroupCount = 0;

		metadataFile.read((char*)&buttonGroupCount, sizeof(int));

		for (int i = 0; i < buttonGroupCount; i++)
		{
			int buttonGroupScriptNameSize = 0;

			metadataFile.read((char*)&buttonGroupScriptNameSize, sizeof(int));

			std::string buttonGroupScriptName(buttonGroupScriptNameSize, '\0');
			metadataFile.read(&buttonGroupScriptName[0], buttonGroupScriptNameSize);

			int buttonGroupNameSize = 0;

			metadataFile.read((char*)&buttonGroupNameSize, sizeof(int));

			std::string buttonGroupName(buttonGroupNameSize, '\0');
			metadataFile.read(&buttonGroupName[0], buttonGroupNameSize);

			GameButtonGroupUuid buttonGroupUuid;
			char* uuidBytes = new char[uuidSize];
			metadataFile.read(uuidBytes, uuidSize);
			memcpy(&buttonGroupUuid, uuidBytes, uuidSize);
			delete uuidBytes;

			int buttonGroupId = BaseIds::getIntegerFromUuid(buttonGroupUuid);

			NameIdPair nameId(buttonGroupId, buttonGroupScriptName);

			BaseIds::idNames.push_back(nameId);

			buttonManager->addButtonGroupNameIdMapping(buttonGroupId, buttonGroupUuid, buttonGroupName);
		}

		// Load the entity classifications.
		int entityClassificationCount = 0;
		
		metadataFile.read((char*)&entityClassificationCount, sizeof(int));
		
		for (int i = 0; i < entityClassificationCount; i++)
		{
			int entityClassificationScriptNameSize = 0;

			metadataFile.read((char*)&entityClassificationScriptNameSize, sizeof(int));

			std::string entityClassificationScriptName(entityClassificationScriptNameSize, '\0');
			metadataFile.read(&entityClassificationScriptName[0], entityClassificationScriptNameSize);
			
			int entityClassificationNameSize = 0;

			metadataFile.read((char*)&entityClassificationNameSize, sizeof(int));

			std::string entityClassification(entityClassificationNameSize, '\0');
			metadataFile.read(&entityClassification[0], entityClassificationNameSize);
			
			GameButtonUuid entityClassificationUuid;
			char* uuidBytes = new char[uuidSize];
			metadataFile.read(uuidBytes, uuidSize);		
			memcpy(&entityClassificationUuid, uuidBytes, uuidSize);
			delete uuidBytes;

			int entityClassificationId = BaseIds::getIntegerFromUuid(entityClassificationUuid);

			NameIdPair nameId(entityClassificationId,entityClassificationScriptName);

			BaseIds::idNames.push_back(nameId);
		}

		// Load the spawn points.
		int spawnPointCount = 0;
		
		metadataFile.read((char*)&spawnPointCount, sizeof(int));
		
		for (int i = 0; i < spawnPointCount; i++)
		{
			int spawnPointScriptNameSize = 0;

			metadataFile.read((char*)&spawnPointScriptNameSize, sizeof(int));

			std::string spawnPointScriptName(spawnPointScriptNameSize, '\0');
			metadataFile.read(&spawnPointScriptName[0], spawnPointScriptNameSize);
			
			int spawnPointNameSize = 0;

			metadataFile.read((char*)&spawnPointNameSize, sizeof(int));

			std::string spawnPoint(spawnPointNameSize, '\0');
			metadataFile.read(&spawnPoint[0], spawnPointNameSize);
			
			GameButtonUuid spawnPointUuid;
			char* uuidBytes = new char[uuidSize];
			metadataFile.read(uuidBytes, uuidSize);		
			memcpy(&spawnPointUuid, uuidBytes, uuidSize);
			delete uuidBytes;

			int spawnPointId = BaseIds::getIntegerFromUuid(spawnPointUuid);

			NameIdPair nameId(spawnPointId, spawnPointScriptName);

			BaseIds::idNames.push_back(nameId);
		}

		metadataFile.close();
	}
	

	// Load the audio asset files.	
	for (boost::filesystem::directory_iterator itr(assets); itr != end_itr; ++itr)
    {
		boost::filesystem::path filepath = itr->path();
		boost::filesystem::path extension = itr->path().extension();

        if (extension == ".aa") 
		{
			if (debugger_->debugLevel >= 1)
			{
				std::cout << "Loading audio file " << filepath << "..." << std::endl;
			}

			std::ifstream audioAssetFile;
	
			audioAssetFile.open(itr->path().c_str(), std::ios::in | std::ios::binary);

			if (audioAssetFile.is_open())
			{
				int fileMajorVersion = 0;
				audioAssetFile.read((char*)&fileMajorVersion, sizeof(int));

				int fileMinorVersion = 0;
				audioAssetFile.read((char*)&fileMinorVersion, sizeof(int));
		
				int fileRevisionVersion = 0;
				audioAssetFile.read((char*)&fileRevisionVersion, sizeof(int));

				int audioNameSize = 0;

				audioAssetFile.read((char*)&audioNameSize, sizeof(int));

				std::string audioName(audioNameSize, '\0');
				audioAssetFile.read(&audioName[0], audioNameSize);
			
				boost::uuids::uuid audioAssetUuid;
				char* uuidBytes = new char[uuidSize];
				audioAssetFile.read(uuidBytes, uuidSize);		
				memcpy(&audioAssetUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				boost::uuids::uuid audioDataUuid;
				uuidBytes = new char[uuidSize];
				audioAssetFile.read(uuidBytes, uuidSize);		
				memcpy(&audioDataUuid, uuidBytes, uuidSize);
				delete uuidBytes;
				
				int groupNameSize = 0;

				audioAssetFile.read((char*)&groupNameSize, sizeof(int));

				std::string groupName(groupNameSize, '\0');
				audioAssetFile.read(&groupName[0], groupNameSize);
			
				bool loop = false;

				if (groupName == "Music")
				{
					loop = true;
				}

				//audioFile.read((char*)&loop, sizeof(bool));
			
				float volume = 1.0;
				//audioFile.read((char*)&volume, sizeof(double));

				// Read the bitmap file to a byte array.
				std::string audioFileName = ".\\Data\\Assets\\" + boost::lexical_cast<std::string>(audioDataUuid) + ".ar";

				std::ifstream audioFile;
	
				audioFile.open(audioFileName.c_str(), std::ios::in | std::ios::binary);
				
				if (audioFile.is_open())
				{
					int fileMajorVersion = 0;
					audioFile.read((char*)&fileMajorVersion, sizeof(int));

					int fileMinorVersion = 0;
					audioFile.read((char*)&fileMinorVersion, sizeof(int));
		
					int fileRevisionVersion = 0;
					audioFile.read((char*)&fileRevisionVersion, sizeof(int));

					// Don't actually need this ID here. Just ignore it
					boost::uuids::uuid audioUuid2;
					uuidBytes = new char[uuidSize];
					audioFile.read(uuidBytes, uuidSize);		
					memcpy(&audioUuid2, uuidBytes, uuidSize);
					delete uuidBytes;

					int audioDataSize = 0;
					audioFile.read((char*)&audioDataSize, sizeof(int));
			
					char* audioBuffer = new char[audioDataSize];
		
					audioFile.read((char*)audioBuffer, audioDataSize);
			
					int newAudioId = audioPlayer_->loadAudioResource(audioName, audioBuffer, audioDataSize, groupName);
					
					AssetId editorAudioId = BaseIds::getIntegerFromUuid(audioAssetUuid);

					audioPlayer_->audioIdMap_[editorAudioId] = newAudioId;

					audioFile.close();
				}

				audioAssetFile.close();
			}
		}
	}
	
	entityTemplateIdMap_.clear();
		
	// Load the actor files.	
	for (boost::filesystem::directory_iterator itr(assets); itr != end_itr; ++itr)
    {
		boost::filesystem::path filepath = itr->path();
		boost::filesystem::path extension = itr->path().extension();

        if (extension == ".ae") 
		{
			if (debugger_->debugLevel >= 1)
			{
				std::cout << "Loading actor " << filepath << "..." << std::endl;
			}

			std::ifstream actorFile;
	
			actorFile.open(filepath.c_str(), std::ios::in | std::ios::binary);

			if (actorFile.is_open())
			{
				int fileMajorVersion = 0;
				actorFile.read((char*)&fileMajorVersion, sizeof(int));

				int fileMinorVersion = 0;
				actorFile.read((char*)&fileMinorVersion, sizeof(int));
		
				int fileRevisionVersion = 0;
				actorFile.read((char*)&fileRevisionVersion, sizeof(int));

				newEntityTemplate = boost::shared_ptr<EntityTemplate>(new EntityTemplate());
				
				newEntityTemplate->renderer_ = renderer_;

				newEntityTemplate->setClassification(ENTITY_CLASSIFICATION_ACTOR);
				
				int actorNameSize = 0;

				actorFile.read((char*)&actorNameSize, sizeof(int));

				std::string actorName(actorNameSize, '\0');
				actorFile.read(&actorName[0], actorNameSize);
			
				int scriptNameSize = 0;

				actorFile.read((char*)&scriptNameSize, sizeof(int));

				std::string scriptName(scriptNameSize, '\0');
				actorFile.read(&scriptName[0], scriptNameSize);
			
				int pythonVariableNameSize = 0;

				actorFile.read((char*)&pythonVariableNameSize, sizeof(int));

				std::string pythonVariableName(pythonVariableNameSize, '\0');
				actorFile.read(&pythonVariableName[0], pythonVariableNameSize);
			
				newEntityTemplate->setTypeName(actorName);
				newEntityTemplate->setScriptName(scriptName);

				int stageHeight = 0;
				int stageWidth = 0;

				unsigned int stageOriginInt = 0;

				int stageBackgroundDepth = 0;

				int pivotX = 0;
				int pivotY = 0;

				bool acceptUserInput = false;
				
				boost::uuids::uuid actorUuid;
				char* uuidBytes = new char[uuidSize];
				actorFile.read(uuidBytes, uuidSize);		
				memcpy(&actorUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				EntityTypeId actorId = BaseIds::getIntegerFromUuid(actorUuid);

				NameIdPair nameId(actorId, pythonVariableName);

				BaseIds::idNames.push_back(nameId);

				actorFile.read((char*)&stageWidth, sizeof(int));
				actorFile.read((char*)&stageHeight, sizeof(int));
				actorFile.read((char*)&stageOriginInt, sizeof(unsigned int));
				actorFile.read((char*)&stageBackgroundDepth, sizeof(int));

				actorFile.read((char*)&pivotX, sizeof(int));
				actorFile.read((char*)&pivotY, sizeof(int));
				
				StageMetadataPtr stageMetadata = newEntityTemplate->getStageMetadata();


				stageMetadata->setHeight(stageHeight);
				stageMetadata->setWidth(stageWidth);
				stageMetadata->setOrigin((StageOrigin)stageOriginInt);
				stageMetadata->setBackgroundDepth(stageBackgroundDepth);

				
				// Using the origin, calculate the stage offset from the position.
				int offsetToOriginX = 0;
				int offsetToOriginY = 0;

				switch (stageMetadata->getOrigin())
				{
				case STAGE_ORIGIN_TOP_LEFT:
					// No offset.
					break;

				case STAGE_ORIGIN_TOP_MIDDLE:

					offsetToOriginX = (int)(stageWidth / 2);

					break;

				case STAGE_ORIGIN_TOP_RIGHT:

					offsetToOriginX = stageWidth;

					break;

				case STAGE_ORIGIN_MIDDLE_LEFT:

					offsetToOriginY = (int)(stageHeight / 2);

					break;

				case STAGE_ORIGIN_CENTER:

					offsetToOriginX = (int)(stageWidth / 2);

					offsetToOriginY = (int)(stageHeight / 2);

					break;

				case STAGE_ORIGIN_MIDDLE_RIGHT:

					offsetToOriginX = stageWidth;

					offsetToOriginY = (int)(stageHeight / 2);

					break;

				case STAGE_ORIGIN_BOTTOM_LEFT:

					offsetToOriginY = stageHeight;

					break;

				case STAGE_ORIGIN_BOTTOM_MIDDLE:

					offsetToOriginX = (int)(stageWidth / 2);

					offsetToOriginY = stageHeight;

					break;

				case STAGE_ORIGIN_BOTTOM_RIGHT:

					offsetToOriginX = stageWidth;

					offsetToOriginY = stageHeight;

					break;
				}
				
				// Set the stage position (relative to the origin, i.e. the position point).
				stageMetadata->getPosition()->setX(-offsetToOriginX);
				stageMetadata->getPosition()->setY(-offsetToOriginY);
				
				stageMetadata->getRotationOperation()->getPivotPoint()->setX(pivotX);
				stageMetadata->getRotationOperation()->getPivotPoint()->setY(pivotY);

				int tagSize = 0;

				actorFile.read((char*)&tagSize, sizeof(int));

 				std::string tag(tagSize, '\0');
				actorFile.read(&tag[0], tagSize);
			
				boost::uuids::uuid actorClassificationUuid;
				uuidBytes = new char[uuidSize];
				actorFile.read(uuidBytes, uuidSize);		
				memcpy(&actorClassificationUuid, uuidBytes, uuidSize);
				delete uuidBytes;
				
				EntityTypeId actorClassificationId = BaseIds::getIntegerFromUuid(actorClassificationUuid);

				entityMetadataContainer_->addEntityMetadata(actorId, actorClassificationId, tag);
				
				bool keepRoomActive = false;
				actorFile.read((char*)&keepRoomActive, sizeof(bool));
				
				newEntityTemplate->setKeepRoomActive(keepRoomActive);

				int stateCount = 0;
				actorFile.read((char*)&stateCount, sizeof(int));

#if defined(_DEBUG)	

				if (actorId == 695)
				{
					bool debug = true;
				}

#endif

				for (int j = 0; j < stateCount; j++)
				{
					int stateNameSize = 0;

					actorFile.read((char*)&stateNameSize, sizeof(int));

					std::string stateName(stateNameSize, '\0');

					actorFile.read(&stateName[0], stateNameSize);

					bool isInitialState = false;

					actorFile.read((char*)&isInitialState, sizeof(bool));

					if (isInitialState == true)
					{
						newEntityTemplate->setInitialStateIndex(j);
					}

					newStageElements = StageElementsPtr(new StageElements(stateName));
					
					newStageElements->stageMetadata_ = newEntityTemplate->getStageMetadata();

					newStageElements->animationManager_ = animationManager_;

					newStageElements->anchorPointManager_ = anchorPointManager_;

					newStageElements->debugger_ = debugger_;

					newStageElements->renderer_ = renderer_;

					newStageElements->hitboxManager_ = hitboxManager_;

					//newStageElements->ownerMetadata_ = hitboxManager_;

					int hitboxCount = 0;

					actorFile.read((char*)&hitboxCount, sizeof(int));

					for (int k = 0; k < hitboxCount; k++)
					{
						int top = 0;
						int left = 0;
						int height = 0;
						int width = 0;

						actorFile.read((char*)&top, sizeof(int));
						actorFile.read((char*)&left, sizeof(int));
						actorFile.read((char*)&height, sizeof(int));
						actorFile.read((char*)&width, sizeof(int));
						
						boost::uuids::uuid hitboxIdentityUuid;
						char* uuidBytes = new char[uuidSize];
						actorFile.read(uuidBytes, uuidSize);		
						memcpy(&hitboxIdentityUuid, uuidBytes, uuidSize);
						delete uuidBytes;

						unsigned int priority = 0;
						actorFile.read((char*)&priority, sizeof(unsigned int));
					
						bool isSolid = false;
						actorFile.read((char*)&isSolid, sizeof(bool));

						float rotationDegrees = 0;
						actorFile.read((char*)&rotationDegrees, sizeof(float));

						if (hitboxIdentityUuid != boost::uuids::nil_uuid())
						{
							HitboxIdentity hitboxIdentityId = BaseIds::getIntegerFromUuid(hitboxIdentityUuid);

							boost::shared_ptr<Hitbox> hitbox = boost::shared_ptr<Hitbox>(new Hitbox(left, top, height, width));
							hitbox->setIdentity(hitboxIdentityId);

							hitbox->setIsSolid(isSolid);

							hitbox->setBaseRotationDegrees(rotationDegrees);
						
							int hitboxId = hitboxManager_->addHitbox(hitbox);
							
							// Add this hitbox's ID/index to the states's list of hitboxes it points to.
							newStageElements->addHitboxReference(hitboxId);
						}
					}

					int animationSlotCount = 0;
					actorFile.read((char*)&animationSlotCount, sizeof(int));

					bool singleFrame = true;

					for (int k = 0; k < animationSlotCount; k++)
					{
						int animationSlotNameSize = 0;

						actorFile.read((char*)&animationSlotNameSize, sizeof(int));

						std::string animationSlotName(animationSlotNameSize, '\0');
						actorFile.read(&animationSlotName[0], animationSlotNameSize);
						
						boost::uuids::uuid animationUuid;
						char* uuidBytes = new char[uuidSize];
						actorFile.read(uuidBytes, uuidSize);		
						memcpy(&animationUuid, uuidBytes, uuidSize);
						delete uuidBytes;

						AssetId animationId = -1;
						
						if (animationUuid != boost::uuids::nil_uuid())
						{
							animationId = BaseIds::getIntegerFromUuid(animationUuid);
						}

						bool background = false;
						actorFile.read((char*)&background, sizeof(bool));

						int positionX = 0;
						int positionY = 0;
						actorFile.read((char*)&positionX, sizeof(int));
						actorFile.read((char*)&positionY, sizeof(int));
						
						float hueRed = 0.0f;
						float hueBlue = 0.0f;
						float hueGreen = 0.0f;
						float hueAlpha = 0.0f;

						actorFile.read((char*)&hueRed, sizeof(float));
						actorFile.read((char*)&hueGreen, sizeof(float));
						actorFile.read((char*)&hueBlue, sizeof(float));
						actorFile.read((char*)&hueAlpha, sizeof(float));

						ColorRgbaPtr hueColor = ColorRgbaPtr(new ColorRgba(hueRed, hueGreen, hueBlue, hueAlpha));

						float blendRed = 0.0f;
						float blendBlue = 0.0f;
						float blendGreen = 0.0f;
						float blendAlpha = 0.0f;
						float blendPercent = 0.0f;

						actorFile.read((char*)&blendRed, sizeof(float));
						actorFile.read((char*)&blendGreen, sizeof(float));
						actorFile.read((char*)&blendBlue, sizeof(float));
						actorFile.read((char*)&blendAlpha, sizeof(float));
						actorFile.read((char*)&blendPercent, sizeof(float));

						ColorRgbaPtr blendColor = ColorRgbaPtr(new ColorRgba(blendRed, blendGreen, blendBlue, blendAlpha));

						int pivotX = 0;
						int pivotY = 0;

						actorFile.read((char*)&pivotX, sizeof(int));
						actorFile.read((char*)&pivotY, sizeof(int));

						float alphaGradientFrom = 0.0f;
						float alphaGradientTo = 0.0f;
						float alphaGradientRadius = 0.0f;

						actorFile.read((char*)&alphaGradientFrom, sizeof(float));
						actorFile.read((char*)&alphaGradientTo, sizeof(float));
						actorFile.read((char*)&alphaGradientRadius, sizeof(float));

						int alphaGradientRadialCenterX = 0;
						int alphaGradientRadialCenterY = 0;

						actorFile.read((char*)&alphaGradientRadialCenterX, sizeof(int));
						actorFile.read((char*)&alphaGradientRadialCenterY, sizeof(int));


						unsigned int direction = 0;

						actorFile.read((char*)&direction, sizeof(unsigned int));

						AlphaGradientDirection gradientDirection = (AlphaGradientDirection)direction;
						
						unsigned int originInt = 0;

						actorFile.read((char*)&originInt, sizeof(unsigned int));

						AnimationSlotOrigin origin = (AnimationSlotOrigin)originInt;

						int animationSlotNextStateNameSize = 0;

						actorFile.read((char*)&animationSlotNextStateNameSize, sizeof(int));

						std::string animationSlotNextStateName(animationSlotNextStateNameSize, '\0');
						actorFile.read(&animationSlotNextStateName[0], animationSlotNextStateNameSize);

						int framesPerSecond = 0;
						actorFile.read((char*)&framesPerSecond, sizeof(int));

						unsigned int style = 0;
						actorFile.read((char*)&style, sizeof(unsigned int));
						
						float outlineRed = 0.0f;
						float outlineGreen = 0.0f;
						float outlineBlue = 0.0f;
						float outlineAlpha = 0.0f;

						actorFile.read((char*)&outlineRed, sizeof(float));
						actorFile.read((char*)&outlineGreen, sizeof(float));
						actorFile.read((char*)&outlineBlue, sizeof(float));
						actorFile.read((char*)&outlineAlpha, sizeof(float));

						ColorRgbaPtr outlineColor = ColorRgbaPtr(new ColorRgba(outlineRed, outlineGreen, outlineBlue, outlineAlpha));

						float rotation = 0.0f;
						
						int newAnimationId = -1;
						
						if (animationId >= 0)
						{
							newAnimationId = animationManager_->animationIdMap_[animationId];

							// Check the number of frames in each animation this state points to.
							// If any of them are greater than 1, then the singleFrame variable should be set to false
							boost::shared_ptr<Animation> a = animationManager_->getAnimationByIndex(newAnimationId);

							if (a->getFrameCount() > 1 && singleFrame == true)
							{
								singleFrame = false;
							}
						}
						else
						{
							singleFrame = false;
						}
						
						//newStageElements->addAnimationSlotInternal(animationSlotName,          positionX,                  positionY,
						//								           hueColor,                   blendColor,                 blendPercent,
						//								           rotation,                   framesPerSecond,            pivotX,
						//	                                       pivotY,                     alphaGradientFrom,          alphaGradientTo, 
						//								           alphaGradientRadialCenterX, alphaGradientRadialCenterY, alphaGradientRadius,
						//								           gradientDirection,          origin,                     animationSlotNextStateName, 
						//	                                       (AnimationStyle)style);
						
						newStageElements->addAnimationSlotInternal(animationSlotName,          positionX,                  positionY,
														           hueColor,                   blendColor,                 blendPercent,
														           rotation,                   framesPerSecond,            pivotX,
							                                       pivotY,                     origin,                     animationSlotNextStateName, 
							                                       (AnimationStyle)style,      outlineColor,			   background);

						newStageElements->assignAnimationByIdToSlotByIndexInternal(k, newAnimationId, false);
					}

					newStageElements->setSingleFrame(singleFrame);

					int newStateMachineStateId = newEntityTemplate->addStageElements(newStageElements);

					// Note: There's nothing that needs to reference states, so
					// there is no old ID to map.
				}

				int propertyCount = 0;
				actorFile.read((char*)&propertyCount, sizeof(int));

				for (int j = 0; j < propertyCount; j++)
				{
					int propertyNameSize = 0;

					actorFile.read((char*)&propertyNameSize, sizeof(int));

					std::string propertyName(propertyNameSize, '\0');
					actorFile.read(&propertyName[0], propertyNameSize);
							
					int propertyValueSize = 0;

					actorFile.read((char*)&propertyValueSize, sizeof(int));

					std::string propertyValue(propertyValueSize, '\0');
					actorFile.read(&propertyValue[0], propertyValueSize);

				}

				// Finally, add this descritor to the vector.
				entityTemplates_.push_back(newEntityTemplate);

				// Map the entityTemplate index to the entity ID.
				entityTemplateIdMap_[actorId] = entityTemplates_.size() - 1;

				actorFile.close();
			}
		}
	}

	// Load the event files.	
	for (boost::filesystem::directory_iterator itr(assets); itr != end_itr; ++itr)
    {
		boost::filesystem::path filepath = itr->path();
		boost::filesystem::path extension = itr->path().extension();

        if (extension == ".ee") 
		{
			if (debugger_->debugLevel >= 1)
			{
				std::cout << "Loading event " << filepath << "..." << std::endl;
			}

			std::ifstream eventFile;

			eventFile.open(itr->path().c_str(), std::ios::in | std::ios::binary);

			if (eventFile.is_open())
			{
				int fileMajorVersion = 0;
				eventFile.read((char*)&fileMajorVersion, sizeof(int));

				int fileMinorVersion = 0;
				eventFile.read((char*)&fileMinorVersion, sizeof(int));
		
				int fileRevisionVersion = 0;
				eventFile.read((char*)&fileRevisionVersion, sizeof(int));

				newEntityTemplate = boost::shared_ptr<EntityTemplate>(new EntityTemplate());
				
				newEntityTemplate->renderer_ = renderer_;

				newEntityTemplate->setClassification(ENTITY_CLASSIFICATION_EVENT);

				int eventNameSize = 0;

				eventFile.read((char*)&eventNameSize, sizeof(int));

				std::string eventName(eventNameSize, '\0');
				eventFile.read(&eventName[0], eventNameSize);
			
				int scriptNameSize = 0;

				eventFile.read((char*)&scriptNameSize, sizeof(int));

				std::string scriptName(scriptNameSize, '\0');
				eventFile.read(&scriptName[0], scriptNameSize);
			
				int pythonVariableNameSize = 0;

				eventFile.read((char*)&pythonVariableNameSize, sizeof(int));

				std::string pythonVariableName(pythonVariableNameSize, '\0');
				eventFile.read(&pythonVariableName[0], pythonVariableNameSize);
			
				newEntityTemplate->setTypeName(eventName);				
				newEntityTemplate->setScriptName(scriptName);

				boost::uuids::uuid eventUuid;
				char* uuidBytes = new char[uuidSize];
				eventFile.read(uuidBytes, uuidSize);		
				memcpy(&eventUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				EntityTypeId eventId = BaseIds::getIntegerFromUuid(eventUuid);
				
				NameIdPair nameId(eventId, pythonVariableName);

				BaseIds::idNames.push_back(nameId);
				
				int tagSize = 0;

				std::string tag(tagSize, '\0');
				eventFile.read(&tag[0], tagSize);
			
				boost::uuids::uuid eventClassificationUuid;
				uuidBytes = new char[uuidSize];
				eventFile.read(uuidBytes, uuidSize);		
				memcpy(&eventClassificationUuid, uuidBytes, uuidSize);
				delete uuidBytes;
				
				EntityTypeId eventClassificationId = BaseIds::getIntegerFromUuid(eventClassificationUuid);
				
				entityMetadataContainer_->addEntityMetadata(eventId, eventClassificationId, tag);

				int propertyCount = 0;
				eventFile.read((char*)&propertyCount, sizeof(int));

				for (int j = 0; j < propertyCount; j++)
				{
					int propertyNameSize = 0;

					eventFile.read((char*)&propertyNameSize, sizeof(int));

					std::string propertyName(propertyNameSize, '\0');
					eventFile.read(&propertyName[0], propertyNameSize);
							
					int propertyValueSize = 0;

					eventFile.read((char*)&propertyValueSize, sizeof(int));

					std::string propertyValue(propertyValueSize, '\0');
					eventFile.read(&propertyValue[0], propertyValueSize);
				}

				// Finally, add this entityTemplate to the vector.
				entityTemplates_.push_back(newEntityTemplate);

				// Map the entityTemplate index to the sprite ID.
				entityTemplateIdMap_[eventId] = entityTemplates_.size() - 1;

				eventFile.close();
			}
		}
	}

	// Load the HUD element files.	
	for (boost::filesystem::directory_iterator itr(assets); itr != end_itr; ++itr)
    {
		boost::filesystem::path filepath = itr->path();
		boost::filesystem::path extension = itr->path().extension();

        if (extension == ".he") 
		{
			if (debugger_->debugLevel >= 1)
			{
				std::cout << "Loading hud element " << filepath << "..." << std::endl;
			}

			std::ifstream hudElementFile;
	
			hudElementFile.open(itr->path().c_str(), std::ios::in | std::ios::binary);

			if (hudElementFile.is_open())
			{
				int fileMajorVersion = 0;
				hudElementFile.read((char*)&fileMajorVersion, sizeof(int));

				int fileMinorVersion = 0;
				hudElementFile.read((char*)&fileMinorVersion, sizeof(int));
		
				int fileRevisionVersion = 0;
				hudElementFile.read((char*)&fileRevisionVersion, sizeof(int));

				newEntityTemplate = boost::shared_ptr<EntityTemplate>(new EntityTemplate());
				
				newEntityTemplate->renderer_ = renderer_;

				newEntityTemplate->setClassification(ENTITY_CLASSIFICATION_HUDELEMENT);

				int hudElementNameSize = 0;

				hudElementFile.read((char*)&hudElementNameSize, sizeof(int));

				std::string hudElementName(hudElementNameSize, '\0');
				hudElementFile.read(&hudElementName[0], hudElementNameSize);
			
				int scriptNameSize = 0;

				hudElementFile.read((char*)&scriptNameSize, sizeof(int));

				std::string scriptName(scriptNameSize, '\0');
				hudElementFile.read(&scriptName[0], scriptNameSize);
			
				int pythonVariableNameSize = 0;

				hudElementFile.read((char*)&pythonVariableNameSize, sizeof(int));

				std::string pythonVariableName(pythonVariableNameSize, '\0');
				hudElementFile.read(&pythonVariableName[0], pythonVariableNameSize);
			
				boost::uuids::uuid hudElementUuid;
				char* uuidBytes = new char[uuidSize];
				hudElementFile.read(uuidBytes, uuidSize);		
				memcpy(&hudElementUuid, uuidBytes, uuidSize);
				delete uuidBytes;
				
				EntityTypeId hudElementId = BaseIds::getIntegerFromUuid(hudElementUuid);
				
				NameIdPair nameId(hudElementId, pythonVariableName);

				BaseIds::idNames.push_back(nameId);

				newEntityTemplate->setTypeName(hudElementName);
				newEntityTemplate->setScriptName(scriptName);

				int stageHeight = 0;
				int stageWidth = 0;

				unsigned int stageOriginInt;

				int stageBackgroundDepth = 0;

				int pivotX = 0;
				int pivotY = 0;

				bool acceptUserInput = false;

				hudElementFile.read((char*)&stageWidth, sizeof(int));
				hudElementFile.read((char*)&stageHeight, sizeof(int));
				hudElementFile.read((char*)&stageOriginInt, sizeof(unsigned int));
				hudElementFile.read((char*)&stageBackgroundDepth, sizeof(int));
				
				hudElementFile.read((char*)&pivotX, sizeof(int));
				hudElementFile.read((char*)&pivotY, sizeof(int));

				StageMetadataPtr stageMetadata = newEntityTemplate->getStageMetadata();

				stageMetadata->setHeight(stageHeight);
				stageMetadata->setWidth(stageWidth);
				stageMetadata->setOrigin((StageOrigin)stageOriginInt);
				stageMetadata->setBackgroundDepth(stageBackgroundDepth);

				stageMetadata->getRotationOperation()->getPivotPoint()->setX(pivotX);
				stageMetadata->getRotationOperation()->getPivotPoint()->setX(pivotY);
				

				// Using the origin, calculate the stage offset from the position.
				int offsetToOriginX = 0;
				int offsetToOriginY = 0;

				switch (stageMetadata->getOrigin())
				{
				case STAGE_ORIGIN_TOP_LEFT:
					// No offset.
					break;

				case STAGE_ORIGIN_TOP_MIDDLE:

					offsetToOriginX = (int)(stageWidth / 2);

					break;

				case STAGE_ORIGIN_TOP_RIGHT:

					offsetToOriginX = stageWidth;

					break;

				case STAGE_ORIGIN_MIDDLE_LEFT:

					offsetToOriginY = (int)(stageHeight / 2);

					break;

				case STAGE_ORIGIN_CENTER:

					offsetToOriginX = (int)(stageWidth / 2);

					offsetToOriginY = (int)(stageHeight / 2);

					break;

				case STAGE_ORIGIN_MIDDLE_RIGHT:

					offsetToOriginX = stageWidth;

					offsetToOriginY = (int)(stageHeight / 2);

					break;

				case STAGE_ORIGIN_BOTTOM_LEFT:

					offsetToOriginY = stageHeight;

					break;

				case STAGE_ORIGIN_BOTTOM_MIDDLE:

					offsetToOriginX = (int)(stageWidth / 2);

					offsetToOriginY = stageHeight;

					break;

				case STAGE_ORIGIN_BOTTOM_RIGHT:

					offsetToOriginX = stageWidth;

					offsetToOriginY = stageHeight;

					break;
				}

				// Set the stage position (relative to the origin, i.e. the position point).
				stageMetadata->getPosition()->setX(-offsetToOriginX);
				stageMetadata->getPosition()->setY(-offsetToOriginY);


				int tagSize = 0;

				hudElementFile.read((char*)&tagSize, sizeof(int));

				std::string tag(tagSize, '\0');

				if (tagSize > 0)
				{
					hudElementFile.read(&tag[0], tagSize);
				}

				boost::uuids::uuid hudElementClassificationUuid;
				uuidBytes = new char[uuidSize];
				hudElementFile.read(uuidBytes, uuidSize);		
				memcpy(&hudElementClassificationUuid, uuidBytes, uuidSize);
				delete uuidBytes;
				
				EntityTypeId hudElementClassificationId = BaseIds::getIntegerFromUuid(hudElementClassificationUuid);
				
				entityMetadataContainer_->addEntityMetadata(hudElementId, hudElementClassificationId, tag);

				int stateCount = 0;
				hudElementFile.read((char*)&stateCount, sizeof(int));

				for (int j = 0; j < stateCount; j++)
				{
					int stateNameSize = 0;

					hudElementFile.read((char*)&stateNameSize, sizeof(int));

					std::string stateName(stateNameSize, '\0');
					hudElementFile.read(&stateName[0], stateNameSize);
					
					bool isInitialState = false;

					hudElementFile.read((char*)&isInitialState, sizeof(bool));
					
					if (isInitialState == true)
					{
						newEntityTemplate->setInitialStateIndex(j);
					}

					newStageElements = StageElementsPtr(new StageElements(stateName));

					newStageElements->animationManager_ = animationManager_;

					newStageElements->anchorPointManager_ = anchorPointManager_;

					newStageElements->debugger_ = debugger_;

					newStageElements->renderer_ = renderer_;

					newStageElements->hitboxManager_ = hitboxManager_;
					
					int animationSlotCount = 0;

					hudElementFile.read((char*)&animationSlotCount, sizeof(int));

					bool singleFrame = true;

					for (int k = 0; k < animationSlotCount; k++)
					{
						int animationSlotNameSize = 0;

						hudElementFile.read((char*)&animationSlotNameSize, sizeof(int));

						std::string animationSlotName(animationSlotNameSize, '\0');
						hudElementFile.read(&animationSlotName[0], animationSlotNameSize);
						
						boost::uuids::uuid animationUuid;
						char* uuidBytes = new char[uuidSize];
						hudElementFile.read(uuidBytes, uuidSize);		
						memcpy(&animationUuid, uuidBytes, uuidSize);
						delete uuidBytes;

						AssetId animationId = -1;

						if (animationUuid != boost::uuids::nil_uuid())
						{
							animationId = BaseIds::getIntegerFromUuid(animationUuid);
						}

						bool background = false;
						hudElementFile.read((char*)&background, sizeof(bool));

						int positionX = 0;
						int positionY = 0;
						hudElementFile.read((char*)&positionX, sizeof(int));
						hudElementFile.read((char*)&positionY, sizeof(int));
						
						float hueRed = 0.0f;
						float hueBlue = 0.0f;
						float hueGreen = 0.0f;
						float hueAlpha = 0.0f;

						hudElementFile.read((char*)&hueRed, sizeof(float));
						hudElementFile.read((char*)&hueGreen, sizeof(float));
						hudElementFile.read((char*)&hueBlue, sizeof(float));
						hudElementFile.read((char*)&hueAlpha, sizeof(float));
						
						ColorRgbaPtr hueColor = ColorRgbaPtr(new ColorRgba(hueRed, hueGreen, hueBlue, hueAlpha));

						float blendRed = 0.0f;
						float blendBlue = 0.0f;
						float blendGreen = 0.0f;
						float blendAlpha = 0.0f;
						float blendPercent = 0.0f;

						hudElementFile.read((char*)&blendRed, sizeof(float));
						hudElementFile.read((char*)&blendGreen, sizeof(float));
						hudElementFile.read((char*)&blendBlue, sizeof(float));
						hudElementFile.read((char*)&blendAlpha, sizeof(float));
						hudElementFile.read((char*)&blendPercent, sizeof(float));

						ColorRgbaPtr blendColor = ColorRgbaPtr(new ColorRgba(blendRed, blendGreen, blendBlue, blendAlpha));

						int pivotX = 0;
						int pivotY = 0;
						hudElementFile.read((char*)&pivotX, sizeof(int));
						hudElementFile.read((char*)&pivotY, sizeof(int));

						float alphaGradientFrom = 0.0f;
						float alphaGradientTo = 0.0f;
						float alphaGradientRadius = 0.0f;

						hudElementFile.read((char*)&alphaGradientFrom, sizeof(float));
						hudElementFile.read((char*)&alphaGradientTo, sizeof(float));
						hudElementFile.read((char*)&alphaGradientRadius, sizeof(float));

						int alphaGradientRadialCenterX = 0;
						int alphaGradientRadialCenterY = 0;

						hudElementFile.read((char*)&alphaGradientRadialCenterX, sizeof(int));
						hudElementFile.read((char*)&alphaGradientRadialCenterY, sizeof(int));

						unsigned int direction = 0;

						hudElementFile.read((char*)&direction, sizeof(unsigned int));

						AlphaGradientDirection gradientDirection = (AlphaGradientDirection)direction;
						
						unsigned int originInt = 0;

						hudElementFile.read((char*)&originInt, sizeof(unsigned int));

						AnimationSlotOrigin origin = (AnimationSlotOrigin)originInt;

						int animationSlotNextStateNameSize = 0;

						hudElementFile.read((char*)&animationSlotNextStateNameSize, sizeof(int));

						std::string animationSlotNextStateName(animationSlotNextStateNameSize, '\0');
						hudElementFile.read(&animationSlotNextStateName[0], animationSlotNextStateNameSize);

						int framesPerSecond = 0;
						hudElementFile.read((char*)&framesPerSecond, sizeof(int));

						unsigned int style = 0;
						hudElementFile.read((char*)&style, sizeof(unsigned int));

						float outlineRed = 0.0f;
						float outlineGreen = 0.0f;
						float outlineBlue = 0.0f;
						float outlineAlpha = 0.0f;

						hudElementFile.read((char*)&outlineRed, sizeof(float));
						hudElementFile.read((char*)&outlineGreen, sizeof(float));
						hudElementFile.read((char*)&outlineBlue, sizeof(float));
						hudElementFile.read((char*)&outlineAlpha, sizeof(float));

						ColorRgbaPtr outlineColor = ColorRgbaPtr(new ColorRgba(outlineRed, outlineGreen, outlineBlue, outlineAlpha));

						float rotation = 0.0f;
						
						int newAnimationId = -1;
						
						if (animationId >= 0)
						{
							newAnimationId = animationManager_->animationIdMap_[animationId];

							boost::shared_ptr<Animation> a = animationManager_->getAnimationByIndex(newAnimationId);
						}
						
						//newStageElements->addAnimationSlotInternal(animationSlotName,
						//										   positionX, 
						//										   positionY, 
						//										   hueColor, blendColor, blendPercent,
						//										   rotation, 
						//	                                       framesPerSecond,
						//										   pivotX, pivotY,
						//										   alphaGradientFrom, alphaGradientTo, 
						//										   alphaGradientRadialCenterX, alphaGradientRadialCenterY, alphaGradientRadius, 
						//										   gradientDirection,
						//										   origin,
						//	                                       animationSlotNextStateName,
						//	                                       (AnimationStyle)style);

						newStageElements->addAnimationSlotInternal(animationSlotName,
																   positionX, 
																   positionY, 
																   hueColor, blendColor, blendPercent,
																   rotation, 
							                                       framesPerSecond,
																   pivotX, pivotY,
																   origin,
							                                       animationSlotNextStateName,
							                                       (AnimationStyle)style,
																   outlineColor,
																   background);

						newStageElements->assignAnimationByIdToSlotByIndexInternal(k, newAnimationId, false);

						if (newAnimationId >= 0)
						{
							// Check the number of frames in each animation this state points to.
							// If any of them are greater than 1, then the singleFrame variable should be set to false
							boost::shared_ptr<Animation> a = animationManager_->getAnimationByIndex(newAnimationId);

							if (a->getFrameCount() > 1 && singleFrame == true)
							{
								singleFrame = false;
							}
						}
					}

					newStageElements->setSingleFrame(singleFrame);

					int newStateMachineStateID = newEntityTemplate->addStageElements(newStageElements);

					// Note: There's nothing that needs to reference states, so
					// there is no old ID to map.
				}

				int propertyCount = 0;
				hudElementFile.read((char*)&propertyCount, sizeof(int));

				for (int j = 0; j < propertyCount; j++)
				{
					int propertyNameSize = 0;

					hudElementFile.read((char*)&propertyNameSize, sizeof(int));

					std::string propertyName(propertyNameSize, '\0');
					hudElementFile.read(&propertyName[0], propertyNameSize);
							
					int propertyValueSize = 0;

					hudElementFile.read((char*)&propertyValueSize, sizeof(int));

					std::string propertyValue(propertyValueSize, '\0');
					hudElementFile.read(&propertyValue[0], propertyValueSize);
				}

				// Finally, add this descritor to the vector.
				entityTemplates_.push_back(newEntityTemplate);

				// Map the entityTemplate index to the entity ID.
				entityTemplateIdMap_[hudElementId] = entityTemplates_.size() - 1;

				hudElementFile.close();
			}
		}
	}

	// Load the query files.	
	for (boost::filesystem::directory_iterator itr(assets); itr != end_itr; ++itr)
    {
		boost::filesystem::path filepath = itr->path();
		boost::filesystem::path extension = itr->path().extension();

        if (extension == ".qa") 
		{
			if (debugger_->debugLevel >= 1)
			{
				std::cout << "Loading query " << filepath << "..." << std::endl;
			}
			std::ifstream queryFile;
	
			queryFile.open(filepath.c_str(), std::ios::in | std::ios::binary);

			if (queryFile.is_open())
			{
				int fileMajorVersion = 0;
				queryFile.read((char*)&fileMajorVersion, sizeof(int));

				int fileMinorVersion = 0;
				queryFile.read((char*)&fileMinorVersion, sizeof(int));
		
				int fileRevisionVersion = 0;
				queryFile.read((char*)&fileRevisionVersion, sizeof(int));

				int queryNameSize = 0;
				int queryFileNameSize = 0;
				int queryTypeNameSize = 0;

				// Query Name
				queryFile.read((char*)&queryNameSize, sizeof(int));

				std::string queryName(queryNameSize, '\0');
				queryFile.read(&queryName[0], queryNameSize);
			
				// Query File Name	
				queryFile.read((char*)&queryFileNameSize, sizeof(int));

				std::string queryFileName(queryFileNameSize, '\0');
				queryFile.read(&queryFileName[0], queryFileNameSize);
			
				// Query Class Name	
				queryFile.read((char*)&queryTypeNameSize, sizeof(int));

				std::string queryTypeName(queryTypeNameSize, '\0');
				queryFile.read(&queryTypeName[0], queryTypeNameSize);
			
				boost::uuids::uuid queryUuid;
				char* uuidBytes = new char[uuidSize];
				queryFile.read(uuidBytes, uuidSize);		
				memcpy(&queryUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				int queryId = BaseIds::getIntegerFromUuid(queryUuid);

				NameIdPair nameId(queryId, queryName);

				BaseIds::idNames.push_back(nameId);

				ScriptingData scriptingData(queryFileName, queryTypeName);

				BaseIds::idScriptDataMap[queryId] = scriptingData;

				queryFile.close();
			}
		}
	}

	// Load the menu item files.	
	for (boost::filesystem::directory_iterator itr(assets); itr != end_itr; ++itr)
    {
		boost::filesystem::path filepath = itr->path();
		boost::filesystem::path extension = itr->path().extension();

        if (extension == ".mi") 
		{
			if (debugger_->debugLevel >= 1)
			{
				std::cout << "Loading menu item " << filepath << "..." << std::endl;
			}

			std::ifstream menuItemFile;
	
			menuItemFile.open(filepath.c_str(), std::ios::in | std::ios::binary);

			if (menuItemFile.is_open())
			{
				int fileMajorVersion = 0;
				menuItemFile.read((char*)&fileMajorVersion, sizeof(int));

				int fileMinorVersion = 0;
				menuItemFile.read((char*)&fileMinorVersion, sizeof(int));
		
				int fileRevisionVersion = 0;
				menuItemFile.read((char*)&fileRevisionVersion, sizeof(int));

				int menuItemNameSize = 0;
				int menuItemFileNameSize = 0;
				int menuItemTypeNameSize = 0;

				// Menu Item Name
				menuItemFile.read((char*)&menuItemNameSize, sizeof(int));

				std::string menuItemName(menuItemNameSize, '\0');
				menuItemFile.read(&menuItemName[0], menuItemNameSize);
			
				// Menu Item File Name	
				menuItemFile.read((char*)&menuItemFileNameSize, sizeof(int));

				std::string menuItemFileName(menuItemFileNameSize, '\0');
				menuItemFile.read(&menuItemFileName[0], menuItemFileNameSize);
			
				// Menu Item Class Name	
				menuItemFile.read((char*)&menuItemTypeNameSize, sizeof(int));

				std::string menuItemTypeName(menuItemTypeNameSize, '\0');
				menuItemFile.read(&menuItemTypeName[0], menuItemTypeNameSize);
			
				boost::uuids::uuid menuItemUuid;
				char* uuidBytes = new char[uuidSize];
				menuItemFile.read(uuidBytes, uuidSize);		
				memcpy(&menuItemUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				int menuItemId = BaseIds::getIntegerFromUuid(menuItemUuid);

				NameIdPair nameId(menuItemId, menuItemName);

				BaseIds::idNames.push_back(nameId);

				ScriptingData scriptingData(menuItemFileName, menuItemTypeName);

				BaseIds::idScriptDataMap[menuItemId] = scriptingData;

				menuItemFile.close();
			}
		}
	}

	// Load the loading screen files.	
	for (boost::filesystem::directory_iterator itr(assets); itr != end_itr; ++itr)
    {
		boost::filesystem::path filepath = itr->path();
		boost::filesystem::path extension = itr->path().extension();

        if (extension == ".ls") 
		{
			if (debugger_->debugLevel >= 1)
			{
				std::cout << "Loading load screen " << filepath << "..." << std::endl;
			}

			std::ifstream loadingScreenFile;
	
			loadingScreenFile.open(filepath.c_str(), std::ios::in | std::ios::binary);

			if (loadingScreenFile.is_open())
			{
				int fileMajorVersion = 0;
				loadingScreenFile.read((char*)&fileMajorVersion, sizeof(int));

				int fileMinorVersion = 0;
				loadingScreenFile.read((char*)&fileMinorVersion, sizeof(int));
		
				int fileRevisionVersion = 0;
				loadingScreenFile.read((char*)&fileRevisionVersion, sizeof(int));

				int loadingScreenNameSize = 0;
				int loadingScreenFileNameSize = 0;
				int loadingScreenTypeNameSize = 0;

				// Loading Screen Name
				loadingScreenFile.read((char*)&loadingScreenNameSize, sizeof(int));

				std::string loadingScreenName(loadingScreenNameSize, '\0');
				loadingScreenFile.read(&loadingScreenName[0], loadingScreenNameSize);
			
				// Loading Screen File Name	
				loadingScreenFile.read((char*)&loadingScreenFileNameSize, sizeof(int));

				std::string loadingScreenFileName(loadingScreenFileNameSize, '\0');
				loadingScreenFile.read(&loadingScreenFileName[0], loadingScreenFileNameSize);
			
				// Loading Screen Class Name	
				loadingScreenFile.read((char*)&loadingScreenTypeNameSize, sizeof(int));

				std::string loadingScreenTypeName(loadingScreenTypeNameSize, '\0');
				loadingScreenFile.read(&loadingScreenTypeName[0], loadingScreenTypeNameSize);
			
				boost::uuids::uuid loadingScreenUuid;
				char* uuidBytes = new char[uuidSize];
				loadingScreenFile.read(uuidBytes, uuidSize);		
				memcpy(&loadingScreenUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				int loadingScreenId = BaseIds::getIntegerFromUuid(loadingScreenUuid);

				NameIdPair nameId(loadingScreenId, loadingScreenName);

				BaseIds::idNames.push_back(nameId);

				ScriptingData scriptingData(loadingScreenFileName, loadingScreenTypeName);

				BaseIds::idScriptDataMap[loadingScreenId] = scriptingData;

				loadingScreenFile.close();

				boost::shared_ptr<LoadingScreen> newLoadingScreen = boost::shared_ptr<LoadingScreen>(new LoadingScreen());

				newLoadingScreen->id_ = loadingScreenId;

				newLoadingScreen->scriptTypeName_ = loadingScreenTypeName;
				newLoadingScreen->scriptName_ = loadingScreenFileName;
				
				newLoadingScreen->debugger_ = debugger_;
				newLoadingScreen->renderer_ = renderer_;
				newLoadingScreen->textManager_ = textManager_;
				newLoadingScreen->fontManager_ = fontManager_;

				loadingScreenContainer_->addLoadingScreen(newLoadingScreen);
			}
		}
	}

	
	// Load the transition files.	
	for (boost::filesystem::directory_iterator itr(assets); itr != end_itr; ++itr)
    {
		boost::filesystem::path filepath = itr->path();
		boost::filesystem::path extension = itr->path().extension();

        if (extension == ".tr") 
		{
			if (debugger_->debugLevel >= 1)
			{
				std::cout << "Loading transition " << filepath << "..." << std::endl;
			}

			std::ifstream transitionFile;
	
			transitionFile.open(filepath.c_str(), std::ios::in | std::ios::binary);

			if (transitionFile.is_open())
			{
				int fileMajorVersion = 0;
				transitionFile.read((char*)&fileMajorVersion, sizeof(int));

				int fileMinorVersion = 0;
				transitionFile.read((char*)&fileMinorVersion, sizeof(int));
		
				int fileRevisionVersion = 0;
				transitionFile.read((char*)&fileRevisionVersion, sizeof(int));

				int transitionNameSize = 0;
				int transitionFileNameSize = 0;
				int transitionTypeNameSize = 0;

				// Transition Name
				transitionFile.read((char*)&transitionNameSize, sizeof(int));

				std::string transitionName(transitionNameSize, '\0');
				transitionFile.read(&transitionName[0], transitionNameSize);
			
				// Transition File Name	
				transitionFile.read((char*)&transitionFileNameSize, sizeof(int));

				std::string transitionFileName(transitionFileNameSize, '\0');
				transitionFile.read(&transitionFileName[0], transitionFileNameSize);
			
				// Transition Class Name	
				transitionFile.read((char*)&transitionTypeNameSize, sizeof(int));

				std::string transitionTypeName(transitionTypeNameSize, '\0');
				transitionFile.read(&transitionTypeName[0], transitionTypeNameSize);
			
				boost::uuids::uuid transitionUuid;
				char* uuidBytes = new char[uuidSize];
				transitionFile.read(uuidBytes, uuidSize);		
				memcpy(&transitionUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				TransitionId transitionId = BaseIds::getIntegerFromUuid(transitionUuid);

				NameIdPair nameId(transitionId, transitionName);

				BaseIds::idNames.push_back(nameId);

				ScriptingData scriptingData(transitionFileName, transitionTypeName);

				BaseIds::idScriptDataMap[transitionId] = scriptingData;

				transitionFile.close();

				boost::shared_ptr<Transition> newTransition = boost::shared_ptr<Transition>(new Transition());

				newTransition->id_ = transitionId;

				newTransition->scriptTypeName_ = transitionTypeName;
				newTransition->scriptName_ = transitionFileName;
				
				newTransition->debugger_ = debugger_;
				newTransition->renderer_ = renderer_;
				newTransition->textManager_ = textManager_;

				transitionContainer_->addTransition(newTransition);
			}
		}
	}

	
	// Load the particle files.	
	for (boost::filesystem::directory_iterator itr(assets); itr != end_itr; ++itr)
    {
		boost::filesystem::path filepath = itr->path();
		boost::filesystem::path extension = itr->path().extension();

        if (extension == ".pt") 
		{
			if (debugger_->debugLevel >= 1)
			{
				std::cout << "Loading particle " << filepath << "..." << std::endl;
			}

			std::ifstream particleFile;
	
			particleFile.open(filepath.c_str(), std::ios::in | std::ios::binary);

			if (particleFile.is_open())
			{
				int fileMajorVersion = 0;
				particleFile.read((char*)&fileMajorVersion, sizeof(int));

				int fileMinorVersion = 0;
				particleFile.read((char*)&fileMinorVersion, sizeof(int));
		
				int fileRevisionVersion = 0;
				particleFile.read((char*)&fileRevisionVersion, sizeof(int));

				int particleNameSize = 0;
				int particleFileNameSize = 0;
				int particleTypeNameSize = 0;

				// Particle Name
				particleFile.read((char*)&particleNameSize, sizeof(int));

				std::string particleName(particleNameSize, '\0');
				particleFile.read(&particleName[0], particleNameSize);
			
				// Particle File Name	
				particleFile.read((char*)&particleFileNameSize, sizeof(int));

				std::string particleFileName(particleFileNameSize, '\0');
				particleFile.read(&particleFileName[0], particleFileNameSize);
			
				// Particle Class Name	
				particleFile.read((char*)&particleTypeNameSize, sizeof(int));

				std::string particleTypeName(particleTypeNameSize, '\0');
				particleFile.read(&particleTypeName[0], particleTypeNameSize);
			
				boost::uuids::uuid particleUuid;
				char* uuidBytes = new char[uuidSize];
				particleFile.read(uuidBytes, uuidSize);		
				memcpy(&particleUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				int particleId = BaseIds::getIntegerFromUuid(particleUuid);

				NameIdPair nameId(particleId, particleName);

				BaseIds::idNames.push_back(nameId);

				ScriptingData scriptingData(particleFileName, particleTypeName);

				BaseIds::idScriptDataMap[particleId] = scriptingData;

				particleFile.close();
			}
		}
	}

	// Load the particle emitter files.	
	for (boost::filesystem::directory_iterator itr(assets); itr != end_itr; ++itr)
    {
		boost::filesystem::path filepath = itr->path();
		boost::filesystem::path extension = itr->path().extension();

        if (extension == ".pe") 
		{
			if (debugger_->debugLevel >= 1)
			{
				std::cout << "Loading particle emitter " << filepath << "..." << std::endl;
			}

			std::ifstream particleEmitterFile;
	
			particleEmitterFile.open(filepath.c_str(), std::ios::in | std::ios::binary);

			if (particleEmitterFile.is_open())
			{
				int fileMajorVersion = 0;
				particleEmitterFile.read((char*)&fileMajorVersion, sizeof(int));

				int fileMinorVersion = 0;
				particleEmitterFile.read((char*)&fileMinorVersion, sizeof(int));
		
				int fileRevisionVersion = 0;
				particleEmitterFile.read((char*)&fileRevisionVersion, sizeof(int));

				int particleEmitterNameSize = 0;
				int particleEmitterFileNameSize = 0;
				int particleEmitterTypeNameSize = 0;

				// Particle Name
				particleEmitterFile.read((char*)&particleEmitterNameSize, sizeof(int));

				std::string particleEmitterName(particleEmitterNameSize, '\0');
				particleEmitterFile.read(&particleEmitterName[0], particleEmitterNameSize);
			
				// Particle File Name	
				particleEmitterFile.read((char*)&particleEmitterFileNameSize, sizeof(int));

				std::string particleEmitterFileName(particleEmitterFileNameSize, '\0');
				particleEmitterFile.read(&particleEmitterFileName[0], particleEmitterFileNameSize);
			
				// Particle Class Name	
				particleEmitterFile.read((char*)&particleEmitterTypeNameSize, sizeof(int));

				std::string particleEmitterTypeName(particleEmitterTypeNameSize, '\0');
				particleEmitterFile.read(&particleEmitterTypeName[0], particleEmitterTypeNameSize);
			
				boost::uuids::uuid particleEmitterUuid;
				char* uuidBytes = new char[uuidSize];
				particleEmitterFile.read(uuidBytes, uuidSize);		
				memcpy(&particleEmitterUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				int particleEmitterId = BaseIds::getIntegerFromUuid(particleEmitterUuid);

				NameIdPair nameId(particleEmitterId, particleEmitterName);

				BaseIds::idNames.push_back(nameId);

				ScriptingData scriptingData(particleEmitterFileName, particleEmitterTypeName);

				BaseIds::idScriptDataMap[particleEmitterId] = scriptingData;

				particleEmitterFile.close();
			}
		}
	}
}