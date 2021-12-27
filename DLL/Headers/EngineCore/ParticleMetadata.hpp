/* -------------------------------------------------------------------------
** ParticleMetadata.hpp
**
** The ParticleMetadata class contains the meta data associated with a single
** particle entity, such as particle lifetime.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _PARTICLEMETADATA_HPP_
#define _PARTICLEMETADATA_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "EntityMetadata.hpp"

namespace firemelon
{
	enum ParticleOrigin
	{
		PARTICLE_ORIGIN_TOP_LEFT = 0,
		PARTICLE_ORIGIN_CENTER = 1,
		PARTICLE_ORIGIN_TOP_MIDDLE = 2,
		PARTICLE_ORIGIN_TOP_RIGHT = 3,
		PARTICLE_ORIGIN_MIDDLE_LEFT = 4,
		PARTICLE_ORIGIN_MIDDLE_RIGHT = 5,
		PARTICLE_ORIGIN_BOTTOM_LEFT = 6,
		PARTICLE_ORIGIN_BOTTOM_MIDDLE = 7,
		PARTICLE_ORIGIN_BOTTOM_RIGHT = 8,
		PARTICLE_ORIGIN_CUSTOM = 9
	};

	class FIREMELONAPI ParticleMetadata : public EntityMetadata
	{
	public:
		friend class ParticleController;
		friend class ParticleEntityCodeBehind;
		friend class ParticleRenderable;
		friend class Room;

		ParticleMetadata();
		virtual ~ParticleMetadata();

		double	getLifetimePy();
		double	getLifetime();

	private:
		
		double			lifetime_;

		ParticleOrigin	origin_;

		bool			reapplyTransform_;
	};

	typedef boost::shared_ptr<ParticleMetadata> ParticleMetadataPtr;
}

#endif // _ENTITYMETADATA_HPP_