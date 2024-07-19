import { useState } from 'react'
import { statusReq } from '../../../constant/status.Req'
import { CensorShipTutor } from '../../../types/tutor.type'
import { useNavigate } from 'react-router-dom'
import RegisterAsTutor from '../../RegisterAsTutor'
import { useMutation } from '@tanstack/react-query'
import { tutorApi } from '../../../api/tutor.api'
import { toast } from 'react-toastify'
import { studentApi } from '../../../api/student.api'

interface Props {
  tutor: CensorShipTutor
  refetch: () => void
}

export default function TutorApplicationComponent({ tutor, refetch }: Props) {
  const [showDetails, setShowDetails] = useState(false)
  const [isHovered, setIsHovered] = useState(false)
  const [showEditForm, setShowEditForm] = useState(false) // State để theo dõi việc hiển thị form chỉnh sửa

  const toggleDetails = () => {
    setShowDetails(!showDetails)
    // Nếu ấn "Ẩn đơn", ẩn luôn form chỉnh sửa
    if (showEditForm) {
      setShowEditForm(false)
    }
  }

  const handleDeleteRequest = useMutation({
    mutationFn: (id: string) => studentApi.deleteRegisterTutor(id),
    onSuccess: (data) => {
      if (refetch) {
        toast.success(data.data.message)
        refetch()
      }
    }
  })

  const handleDelete = () => {
    const isConfirmed = window.confirm('Bạn có chắc chắn muốn xóa không?')
    if (isConfirmed) {
      handleDeleteRequest.mutate(tutor.id as string)
      refetch()
    }
  }

  const getStatusClasses = (status: string) => {
    switch (status) {
      case statusReq.pending:
        return {
          borderColor: 'border-yellow-600',
          bgColor: 'bg-yellow-600',
          hoverBgColor: 'hover:bg-yellow-700'
        }
      case statusReq.approved:
        return {
          borderColor: 'border-green-600',
          bgColor: 'bg-green-600',
          hoverBgColor: 'hover:bg-green-700'
        }
      case statusReq.reject:
        return {
          borderColor: 'border-red-600',
          bgColor: 'bg-red-600',
          hoverBgColor: 'hover:bg-red-700'
        }
      default:
        return {
          borderColor: 'border-gray-600',
          bgColor: 'bg-gray-600',
          hoverBgColor: 'hover:bg-gray-700'
        }
    }
  }

  const statusClasses = getStatusClasses(tutor.status.toLowerCase())

  const handleEditClick = () => {
    setShowEditForm(!showEditForm) // Khi nhấn "Chỉnh sửa", hiển thị form chỉnh sửa
  }

  const handleRefetch = () => {
    refetch()
    setShowEditForm(false)
  }

  return (
    <div className='container mx-auto p-4'>
      <div
        className={`border-2 ${statusClasses.borderColor} rounded-lg shadow-lg p-2 bg-white relative`}
        onMouseEnter={() => setIsHovered(true)}
        onMouseLeave={() => setIsHovered(false)}
      >
        <div className='flex justify-between items-center h-16'>
          {/* chữ */}
          <div className='flex flex-col'>
            <div className='flex items-center'>
              <div className='text-gray-700 font-medium'>Họ và tên:</div>
              <span className='font-semibold ml-2'>{tutor.fullName}</span>
            </div>
            <div className='flex items-center'>
              <div className='text-gray-700 font-medium'>Ngày sinh:</div>
              <span className='font-semibold ml-2'>{tutor.date_of_birth}</span>
            </div>
            <div className='flex items-center'>
              <div className='text-gray-700 font-medium'>Giới tính:</div>
              <span className='font-semibold ml-2'>{tutor.gender}</span>
            </div>
          </div>

          <button
            className={`rounded-lg h-full w-1/4 flex justify-center items-center ${statusClasses.bgColor} ${statusClasses.hoverBgColor} transition duration-300`}
          >
            <div className='text-white font-bold text-lg py-5'>
              {tutor.status}
            </div>
          </button>
        </div>
        {/* chi tiết đơn */}
        <div
          className={`bg-slate-100 py-auto rounded-lg mt-2 left-0 w-full flex justify-center transition-opacity duration-500 ${
            isHovered ? 'opacity-100' : 'opacity-0 hidden'
          }`}
        >
          <button
            className='mt-4 text-blue-500 hover:text-blue-800 transition duration-300'
            onClick={toggleDetails}
          >
            {showDetails ? 'Ẩn đơn' : 'Chi tiết đơn'}
          </button>
        </div>
        {/*  hiệu ứng */}
        <div
          className={`transition-all duration-500 ease-in-out overflow-hidden ${
            showDetails ? 'max-h-screen' : 'max-h-0'
          }`}
        >
          <div
            className={`mt-4 p-4 border rounded-lg bg-gray-50 shadow-lg ${
              showDetails ? 'block' : 'hidden'
            }`}
          >
            <div className='flex flex-col space-y-2'>
              <div className='flex items-center'>
                <div className='text-gray-700 font-medium'>Giới thiệu:</div>
                <span className='font-semibold ml-2'>{tutor.introduction}</span>
              </div>
              <div className='flex items-center'>
                <div className='text-gray-700 font-medium'>Kinh nghiệm:</div>
                <span className='font-semibold ml-2'>{tutor.experience}</span>
              </div>
              <div className='flex items-center'>
                <div className='text-gray-700 font-medium'>Môn học:</div>
                <span className='font-semibold ml-2'>{tutor.subject}</span>
              </div>
              <div className='flex items-center'>
                <div className='text-gray-700 font-medium'>
                  Kỹ năng đặc biệt:
                </div>
                <span className='font-semibold ml-2'>
                  {tutor.specializedSkills}
                </span>
              </div>
              <div className='flex items-center'>
                <div className='text-gray-700 font-medium'>Loại:</div>
                <span className='font-semibold ml-2'>{tutor.type}</span>
              </div>
              <div className='flex items-center'>
                <div className='text-gray-700 font-medium'>
                  Tên bằng cấp (Chứng chỉ):
                </div>
                <span className='font-semibold ml-2'>
                  {tutor.qualifiCationName}
                </span>
              </div>
              <div className=' items-center border-2  '>
                <div className='text-gray-700 font-medium mb-2'>
                  Ảnh (Bằng cấp/Chứng chỉ):
                </div>
                <img
                  className=' mx-auto border-2 border-black '
                  src={tutor.imageQualification}
                  alt=' Ảnh (Bằng cấp/Chứng chỉ) '
                />
              </div>
            </div>
          </div>
        </div>

        {/* Lý do section */}
        {showDetails && tutor.status.toLowerCase() === statusReq.reject && (
          <div
            className={`mt-4 p-4 border rounded-lg bg-gray-50 shadow-lg transition-all duration-500 ease-in-out`}
          >
            <div className='flex items-center'>
              <div className='text-gray-700 font-medium'>Lý do:</div>
              <span className='font-semibold ml-2 text-red-500'>
                {tutor.reason}
              </span>
            </div>
          </div>
        )}
        {/* Buttons section */}
        {showDetails && (
          <div className='flex justify-between mt-2'>
            {tutor.status.toLowerCase() === statusReq.approved ? (
              <button className='text-white border-2 bg-red-600 border-red-600 rounded-lg w-full h-9 flex justify-center items-center'>
                Xóa
              </button>
            ) : (
              <>
                <button
                  onClick={handleEditClick} // Gọi hàm handleEditClick khi nhấn nút Chỉnh sửa
                  className='text-white text-center bg-yellow-600 border-2 rounded-lg border-yellow-600 w-[49%] h-9 flex justify-center items-center'
                >
                  Chỉnh sửa
                </button>
                <button
                  onClick={handleDelete}
                  className='text-white border-2 bg-red-600 border-red-600 rounded-lg w-[49%] h-9 flex justify-center items-center'
                >
                  Xóa
                </button>
              </>
            )}
          </div>
        )}

        {/* Hiển thị component RegisterAsTutor khi showEditForm là true */}
        {showEditForm && (
          <RegisterAsTutor
            isVisible={showEditForm}
            ReSignUp={tutor.status}
            refetch={handleRefetch}
          />
        )}
      </div>
    </div>
  )
}
