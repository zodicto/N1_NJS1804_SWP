import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { keepPreviousData, useQuery } from '@tanstack/react-query'
import { useContext, useEffect, useState } from 'react'
import { tutorApi } from '../../../../api/tutor.api'
import { AppContext } from '../../../../context/app.context'
import UpdateMajorTT from './components/UpdateMajorTT'
import UpdateProfile from './components/UpdateProfileTT'

interface TutorProfileType {
  speacializedSkill: string
  experience: number
  introduction: string
  subjects: string
  qualifications: {
    id: string
    name: string
    img: string
    type: string
  }
}

export default function ProfileTT() {
  const [showUpdateOptions, setShowUpdateOptions] = useState(false)
  const { profile } = useContext(AppContext)
  const [selectedUpdate, setSelectedUpdate] = useState<string | null>(null)
  const [error, setError] = useState<string | null>(null)

  const {
    data: profileTutor,
    refetch,
    error: queryError
  } = useQuery({
    queryKey: ['Account', profile?.id as string],
    queryFn: () => tutorApi.getProfileTT(profile?.id as string),
    enabled: !!profile?.id,
    placeholderData: keepPreviousData,
    refetchOnWindowFocus: false
  })

  useEffect(() => {
    refetch()
  }, [profile, refetch])

  const handleUpdate = (option: string) => {
    if (selectedUpdate === option) {
      setSelectedUpdate(null)
    } else {
      setSelectedUpdate(option)
    }
  }

  if (error) {
    return (
      <div className='pb-10 rounded-sm bg-transparent px-2 md:px-7 md:pb-20 shadow-black'>
        <div className='border-b border-gray-300 py-6'>
          <h1 className='text-lg font-medium capitalize text-gray-900'>
            Đã xảy ra lỗi, trang sẽ được làm mới...
          </h1>
        </div>
      </div>
    )
  }

  return (
    <div className='pb-10 rounded-sm bg-transparent px-2 md:px-7 md:pb-20 shadow-black'>
      <div className='border-b border-gray-300 py-6'>
        <h1 className='text-lg font-medium capitalize text-gray-900'>
          Hồ Sơ của tôi
        </h1>
        <div className='mt-1 text-sm text-gray-700'>
          Quản lý thông tin hồ sơ để bảo vệ tài khoản
        </div>
      </div>

      <div className='mt-8 flex flex-col md:flex-row md:items-start'>
        <div className='flex-grow'>
          <div className='border rounded-lg p-6 shadow-lg bg-white'>
            <div className='flex flex-row mb-4'>
              <div className='w-1/5 text-right capitalize'>Email</div>
              <div className='w-4/5 pl-5 text-gray-700 text-bold'>
                {profile?.email}
              </div>
            </div>
            <div className='flex mb-4'>
              <div className='w-1/5   text-right capitalize'>Giới thiệu</div>
              <div className='mx-2 w-4/5 rounded-xl border-2 min-h-10 text-left hover:shadow-black hover:shadow-sm pl-2'>
                {profileTutor?.introduction}
              </div>
            </div>

            <div className='flex mb-4'>
              <div className='w-1/5 text-right capitalize'>Môn dạy</div>
              <div className='mx-2  w-4/5 rounded-xl border-2 h-10 text-left hover:shadow-black hover:shadow-sm pl-2'>
                {profileTutor?.subjects}
              </div>
            </div>

            <div className='flex mb-4'>
              <div className='w-1/5 text-right capitalize'>Kinh nghiệm</div>
              <div className='mx-2  w-4/5 rounded-xl border-2 h-10 text-left hover:shadow-black hover:shadow-sm pl-2'>
                {profileTutor?.experience}
              </div>
            </div>

            <div className='flex mb-4'>
              <div className='w-1/5 text-right capitalize'>
                Kỹ năng chuyên môn
              </div>
              <div className='mx-2    w-4/5 rounded-xl border-2 min-h-10 text-left hover:shadow-black hover:shadow-sm pl-2'>
                {profileTutor?.specializedSkills}
              </div>
            </div>

            <div className='w-1/2 mx-auto my-2'>
              <button
                type='button'
                className='w-full p-3 bg-gray-500  text-white rounded-lg hover:bg-black focus:outline-none relative hover:shadow-2xl hover:border-black'
                onClick={() => setShowUpdateOptions(!showUpdateOptions)}
              >
                Cập nhật
                <div className='absolute right-3 top-1/2 transform -translate-y-1/2'>
                  <FontAwesomeIcon
                    icon={showUpdateOptions ? faArrowUp : faArrowDown}
                  />
                </div>
              </button>
            </div>

            {showUpdateOptions && (
              <div className='mt-4 p-4'>
                <button
                  className='w-full p-3 bg-green-700 text-white rounded-lg hover:bg-green-500 focus:outline-none mb-4'
                  onClick={() => handleUpdate('profile')}
                >
                  Cập nhật hồ sơ
                </button>
                <button
                  className='w-full p-3 bg-blue-700 text-white rounded-lg hover:bg-blue-900 focus:outline-none'
                  onClick={() => handleUpdate('major')}
                >
                  Cập nhật chuyên ngành
                </button>
              </div>
            )}

            {selectedUpdate === 'profile' && (
              <div className='mt-4 p-4'>
                <UpdateProfile refetch={refetch} />
              </div>
            )}
            {selectedUpdate === 'major' && (
              <div className='mt-4 p-4'>
                <UpdateMajorTT refetch={refetch} />
              </div>
            )}
          </div>
        </div>

        <div className='hidden md:flex md:w-72 md:border-l md:border-l-gray-200 md:flex-col md:items-center md:justify-center'>
          <div className='my-5 h-64 w-64 overflow-y-auto'>
            <span>Ảnh bằng</span>
            {profileTutor?.qualifications?.length ? (
              profileTutor.qualifications.map((qualification) => (
                <div key={qualification.id} className='mx-2  my-2 border-2 w-'>
                  <label htmlFor=''>{qualification.name}</label>
                  <img
                    src={qualification.img}
                    className='h-full w-full'
                    alt={`Ảnh bằng ${qualification.name}`}
                  />
                </div>
              ))
            ) : (
              <span>Ảnh không phù hợp</span>
            )}
          </div>
        </div>
      </div>
    </div>
  )
}
