import React from 'react'
import { AdminTutorProfile } from '../../types/tutor.type'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import {
  faAudioDescription,
  faBook,
  faImage,
  faPersonHalfDress,
  faSchool,
  faStar,
  faUserGraduate
} from '@fortawesome/free-solid-svg-icons'
import userAvatar from '../../../../OnDemandTutor/src/assets/img/user.svg'
import { useNavigate } from 'react-router-dom'
import { path } from '../../constant/path'

interface Props {
  currentTutor: AdminTutorProfile
}

export default function TutorRender({ currentTutor }: Props) {
  const navigate = useNavigate()

  const handleChangePath = (idTutor: string) => {
    navigate(`${path.services}/${idTutor}`)
  }

  const renderQualificationImages = () => {
    return currentTutor.qualifications.map((qualification, index) => (
      <img
        key={index}
        src={qualification.img}
        alt={`qualification-${index}`}
        className='w-16 h-16 m-1 object-cover rounded'
        onError={(e) => {
          e.currentTarget.src = userAvatar // Fallback to default image if error occurs
        }}
      />
    ))
  }

  return (
    <div className='overflow-y-auto p-4'>
      <div className='flex justify-center py-4'>
        <img
          src={currentTutor.avatar ? currentTutor.avatar : userAvatar}
          alt='gia sư'
          className='w-32 h-32 rounded-full'
          onError={(e) => {
            e.currentTarget.src = userAvatar // Fallback to default image if error occurs
          }}
        />
      </div>
      <div className='mx-4 my-5'>
        <div className='py-2'>
          <div className='justify-start flex pl-2'>
            <div>
              <h1 className='text-2xl font-bold text-start'>
                {currentTutor.fullName}
              </h1>
              <div className='text-lg flex pl-1 pt-2'>
                <FontAwesomeIcon
                  icon={faPersonHalfDress}
                  className='pt-2 h-6'
                />
                <span className='pl-2 pt-1'>{currentTutor.gender}</span>
              </div>
              <div className='text-lg flex pl-1 pt-2 max-w-49'>
                <FontAwesomeIcon
                  icon={faAudioDescription}
                  className='pt-2 h-6'
                />
                <span className='pl-2 pt-1 break-words'>
                  {currentTutor.introduction}
                </span>
              </div>
              <div className='text-lg flex pl-1 pt-2'>
                <FontAwesomeIcon icon={faSchool} className='pt-2 h-6' />
                <span className='pl-2 pt-1'>{currentTutor.subjects}</span>
              </div>
              <div className='text-lg flex pl-1 pt-2'>
                <FontAwesomeIcon icon={faUserGraduate} className='pt-2 h-6' />
                <span className='pl-2 pt-1'>{currentTutor.experience}</span>
              </div>
              <div className='text-lg flex pl-1 pt-2'>
                <FontAwesomeIcon icon={faStar} className='pt-2 h-6' />
                <span className='pl-2 pt-1'>
                  {currentTutor.specializedSkills}
                </span>
              </div>

              <div className='text-lg flex pl-1 pt-2'>
                <FontAwesomeIcon icon={faBook} className='pt-2 h-6' />
                <span className='pl-2 pt-1'>
                  {currentTutor.qualifications.map((qualification, index) => (
                    <span key={index}>{qualification.qualificationName}</span>
                  ))}
                </span>
              </div>
              <div className='text-lg flex flex-wrap pl-1 pt-2'>
                <FontAwesomeIcon icon={faImage} className='pt-2 h-6' />
                <div className='pl-2 pt-1 flex flex-wrap'>
                  {renderQualificationImages()}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div className='mt-5'>
        <button
          type='button'
          onClick={() => handleChangePath(currentTutor.id)}
          className='bg-slate-700 w-full py-2 text-white rounded-lg'
        >
          <span>Xem lớp của gia sư</span>
        </button>
      </div>
    </div>
  )
}
