import { useContext, useState } from 'react'

import { faCircleExclamation } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { AppContext } from '../../../context/app.context'
import { TutorRep, User, UserRep } from '../../../types/user.type'
import Report from '../Report'
import userAvatar from '../../../assets/img/user.svg'
import { roles } from '../../../constant/roles'

interface Props {
  User: UserRep
  Tutor: TutorRep
}

export default function DetailInfor({ User, Tutor }: Props) {
  const user = User
  const tutor = Tutor
  const { profile } = useContext(AppContext)
  const [showForm, setShowForm] = useState(false)

  const handleCloseForm = () => {
    setShowForm(false)
  }

  const handleOpenForm = () => {
    setShowForm(true)
  }

  return (
    <div className='flex justify-around '>
      {/*  user */}
      {/*  Ảnh bản thân */}
      <div className=' border-2   w-[45%]  rounded-2xl hover:shadow-pink-300 hover:shadow-2xl transition-shadow duration-700'>
        <div className=' h-[7rem] pt-2   rounded-2xl'>
          <div className=' py-auto h-44 w-44 mx-auto '>
            {/* ảnh đại diện */}
            <img
              src={user?.avatar ? user.avatar : user?.avatar || userAvatar}
              className=' h-full w-full rounded-full object-cover border-4 border-pink-400 my-auto '
              alt='Profile'
            />
          </div>
        </div>
        <div className='container mx-auto pt-24  border-l-2  rounded-2xl w-[30rem] p-6 bg-transparent  '>
          <div className=''>
            {/* email */}
            <div className='flex items-center text-[1.2rem] my-3'>
              <div className='ml-2 font-sans font-semibold text-pink-600'>
                Email:
              </div>
              <span className='ml-3 text-black font-bold'>{user.email}</span>
            </div>

            {/* tên */}
            <div className='flex items-center text-[1.2rem] my-3'>
              <div className='ml-2 font-sans font-semibold text-pink-600'>
                Họ và tên:
              </div>
              <span className='ml-3 text-black font-bold'>{user.name}</span>
            </div>

            {/* ngày sinh */}
            <div className='flex items-center text-[1.2rem] my-3'>
              <div className='ml-2 font-sans font-semibold text-pink-600'>
                Ngày sinh:
              </div>
              <span className='ml-3 text-black font-bold'>
                {user.date_of_birth}
              </span>
            </div>

            {/* giới tính */}
            <div className='flex items-center text-[1.2rem] my-3'>
              <div className='ml-2 font-sans font-semibold text-pink-600'>
                Giới tính:
              </div>
              <span className='ml-3 text-black font-bold'>{user.gender}</span>
            </div>

            {/* địa chỉ */}
            <div className='flex items-center text-[1.2rem] my-3'>
              <div className='ml-2 font-sans font-semibold text-pink-600'>
                Địa chỉ:
              </div>
              <span className='ml-3 text-black font-bold'>{user.address}</span>
            </div>

            {/* số điện thoại */}
            <div className='flex items-center text-[1.2rem] my-3'>
              <div className='ml-2 font-sans font-semibold text-pink-600'>
                Điện thoại:
              </div>
              <span className='ml-3 text-black font-bold'>{user.phone}</span>
            </div>
          </div>
        </div>
      </div>
      {/*  tutor */}
      <div className='w-[45%] border-2 rounded-2xl hover:shadow-black hover:shadow-2xl transition-shadow duration-700 relative'>
        <div className='h-[7rem] pt-2 bg-black rounded-2xl'>
          <div className='py-auto h-44 w-44 mx-auto'>
            {/* ảnh đại diện */}
            <img
              src={tutor?.avatar ? tutor.avatar : tutor?.avatar || userAvatar}
              className='h-full w-full rounded-full object-cover border-4 my-auto'
              alt='Profile'
            />
          </div>
        </div>
        <div className='container mx-auto pt-24 border-l-2 rounded-2xl w-full p-6 bg-transparent'>
          <div className=''>
            {/* email */}
            <div className='flex items-center text-[1.2rem] my-3'>
              <div className='ml-2 font-sans font-semibold text-pink-600'>
                Email:
              </div>
              <span className='ml-3 text-black font-bold'>{tutor.email}</span>
            </div>
            {/* tên */}
            <div className='flex items-center text-[1.2rem] my-3'>
              <div className='ml-2 font-sans font-semibold text-pink-600'>
                Họ và tên:
              </div>
              <span className='ml-3 text-black font-bold'>{tutor.name}</span>
            </div>
            {/* ngày sinh */}
            <div className='flex items-center text-[1.2rem] my-3'>
              <div className='ml-2 font-sans font-semibold text-pink-600'>
                Ngày sinh:
              </div>
              <span className='ml-3 text-black font-bold'>
                {tutor.date_of_birth}
              </span>
            </div>
            {/* giới tính */}
            <div className='flex items-center text-[1.2rem] my-3'>
              <div className='ml-2 font-sans font-semibold text-pink-600'>
                Giới tính:
              </div>
              <span className='ml-3 text-black font-bold'>{tutor.gender}</span>
            </div>
            {/* địa chỉ */}
            <div className='flex items-center text-[1.2rem] my-3'>
              <div className='ml-2 font-sans font-semibold text-pink-600'>
                Địa chỉ:
              </div>
              <span className='ml-3 text-black font-bold'>{tutor.address}</span>
            </div>
            {/* số điện thoại */}
            <div className='flex items-center text-[1.2rem] my-3'>
              <div className='ml-2 font-sans font-semibold text-pink-600'>
                Điện thoại:
              </div>
              <span className='ml-3 text-black font-bold'>{tutor.phone}</span>
            </div>
          </div>
        </div>
        {/* Nút Report */}
        <div className='absolute top-0 right-0 m-2'>
        {profile?.roles.toLowerCase()=== roles.student ? <>
          <button className='relative group' onClick={() => handleOpenForm()}>
            <FontAwesomeIcon
              icon={faCircleExclamation}
              className='text-red-400 cursor-pointer'
            />
            <div className='absolute left-8 -top-1/3 w-max opacity-0 group-hover:opacity-100 transition-opacity duration-300 bg-gray-800 text-white text-xs rounded p-2'>
              Tố cáo
            </div>
          </button>
        </>:
        <div></div>
        }
          
          {/* Report Form */}
          {showForm && (
            <Report
              onClose={handleCloseForm}
              idAccountTutor={tutor.idAccountTutor as string}
            />
          )}
        </div>
      </div>
    </div>
  )
}
