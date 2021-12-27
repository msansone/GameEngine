class Entity:

    def __init__(self):
        # Every entity has a properties dictionary, whose key/values are set in the editor.
        self._properties = dict()
        self.inputChannel = -1

        return
		
    def centered(self):
	    # Only used for the camera entity.
        return

    def created(self):
        return

    def destroyed(self):
        return

    def roomEntered(self, roomId):
        return

    def roomExited(self, roomId):
        return

    def frameBegin(self):
        return
		
    def update(self, time):
        return

    def preIntegration(self):
        return

    def postIntegration(self):
        return

    def buttonDown(self, buttonCode):
        return

    def buttonUp(self, buttonCode):
        return

    def collision(self, collisionData):
        return

    def collisionEnter(self, collisionData):
        return

    def collisionExit(self, collisionData):
        return

    def resolveCollision(self, collisionData):

        # Default collision resolution should be permeable.
        return self.firemelon.CollisionResolution.PERMEABLE
        
    def getCollisionData(self, hitboxId):
        # Build a collision data object, with all the data the
        # colliding object will need to respond.

        collisionData = self.firemelon.CollisionData()

        return collisionData

    def messageReceived(self, message):
        return
		
    def stateChanged(self, oldStateIndex, newStateIndex):
        return

    def stateEnded(self, stateIndex):
        return

    def frameTriggered(self, frameTriggerSignalId):
        return

    def createDynamicsController(self):
        dynamicsController = self.firemelon.DynamicsController()
        return dynamicsController

    def rendered(self, x, y):
        return