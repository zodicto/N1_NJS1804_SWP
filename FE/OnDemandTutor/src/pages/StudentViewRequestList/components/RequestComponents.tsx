import {
  faBook,
  faCalendarDays,
  faClock,
  faGraduationCap,
  faIdCard,
  faSchool
} from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { useMutation } from '@tanstack/react-query'
import { useContext, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import { studentApi } from '../../../api/student.api'
import { path } from '../../../constant/path'
import { statusReq } from '../../../constant/status.Req'
import { Request as RequestType } from '../../../types/request.type'
import FormRequest from '../../FormRequest/FormRequest'
import Status from '../status'
import { AppContext } from '../../../context/app.context'

interface Props {
  request: RequestType
  refetch?: (() => void) | undefined
}

export default function RequestComponents({ request, refetch }: Props) {
  const [showButtons, setShowButtons] = useState(false)
  const [showForm, setShowForm] = useState(false)
  const navigate = useNavigate()
  const { profile } = useContext(AppContext)

  const handleMouseEnter = () => {
    setShowButtons(true)
  }

  const handleMouseLeave = () => {
    setShowButtons(false)
  }

  const handleOpenPopup = () => {
    setShowForm(true)
  }

  const handleCloseForm = () => {
    setShowForm(false)
  }

  const handleChangePath = (idRe: string) => {
    if (request.status.toLowerCase() === statusReq.approved) {
      navigate(`${path.tutors}/${idRe}`)
    }
  }

  const handleDeleteRequest = useMutation({
    mutationFn: (idRequest: string) =>
      studentApi.deleteRequest(profile?.id as string, idRequest),
    onSuccess: (data) => {
      if (refetch) {
        toast.success(data.data.message)
        refetch() // refetch lại API khi xóa thành công
      }
    }
  })

  const handleDelete = (idRequest: string) => {
    console.log(idRequest)

    const isConfirmed = window.confirm('Bạn có chắc chắn muốn xóa không?')
    if (isConfirmed) {
      handleDeleteRequest.mutate(idRequest)
    }
  }

  const getStatusColor = () => {
    switch (request.status.toLowerCase()) {
      case statusReq.pending:
        return 'border-yellow-600  ' // Đang duyệt
      case statusReq.approved:
        return 'border-green-600  ' // Đã duyệt
      case statusReq.reject:
        return 'border-red-600 ' // Từ chối
      default:
        return 'border-yellow-600 ' // Mặc định
    }
  }

  return (
    <div
      className={`hover:shadow-xl hover:shadow-black rounded-xl my-5 ${
        request.reason && request.status === statusReq.reject
          ? 'h-[22rem]'
          : 'h-[20rem]'
      } border-2 ${getStatusColor()}`}
    >
      <div
        className='container mb-16 p-3 flex border rounded-md relative'
        key={request.idRequest}
        onMouseEnter={handleMouseEnter}
        onMouseLeave={handleMouseLeave}
        onClick={() => handleChangePath(request.idRequest)}
      >
        <div className='w-10/12 bg-slate-100 rounded-xl text-left justify-between text-base p-5 shadow-md'>
          <h1 className='font-bold text-xl text-black text-center'>
            {request.title}
          </h1>
          <div className='flex justify-between'>
            <div className='flex flex-col'>
              <div className='flex items-center'>
                <FontAwesomeIcon icon={faIdCard} className='mr-2' />
                <span>{request.idRequest}</span>
              </div>
              <div className='flex items-center'>
                <FontAwesomeIcon icon={faCalendarDays} className='mr-2' />
                <span>{request.timeTable}</span>
              </div>
              <div className='flex items-center'>
                <FontAwesomeIcon icon={faSchool} className='mr-2' />
                <span>{request.learningMethod}</span>
              </div>
              <div className='flex items-center'>
                <FontAwesomeIcon icon={faGraduationCap} className='mr-2' />
                <span>{request.class}</span>
              </div>
              <div className='flex items-center'>
                <FontAwesomeIcon icon={faBook} className='mr-2' />
                <span>{request.subject}</span>
              </div>
              <div className='flex items-center'>
                <FontAwesomeIcon icon={faClock} className='mr-2' />
                <span>
                  {request.timeStart} tới {request.timeEnd}
                </span>
              </div>
              <div className='flex items-center'>
                <span>Mong muốn: {request.description}</span>
              </div>
            </div>
            <div className='flex flex-col'>
              <div className='font-bold text-lg'>Giá mong muốn</div>
              <div>{request.price}</div>
            </div>
          </div>
        </div>
        <Status request={request} />
        {request.status === statusReq.pending && showButtons && (
          <div
            className={`absolute bottom-0 left-0 right-0 transform transition-transform duration-300 ease-in-out ${
              showButtons
                ? 'translate-y-full flex items-center justify-center bg-transparent bg-opacity-50 rounded-md z-10'
                : 'translate-y-0 hidden'
            }`}
          >
            {' '}
            <div className='flex items-center gap-2 p-3 rounded-md w-full'>
              <button
                onClick={handleOpenPopup}
                className='bg-lime-600 text-white px-4 py-2 rounded-md w-[49%] hover:bg-lime-800  hover:shadow-xl hover:shadow-black'
              >
                Cập nhật
              </button>
              <button
                onClick={() => handleDelete(request.idRequest)}
                className='bg-red-700 text-white px-4 py-2 rounded-md w-[49%] hover:bg-slate-600 hover:shadow-xl hover:shadow-black'
              >
                Xóa
              </button>
            </div>
          </div>
        )}
        {request.status.toLowerCase() === statusReq.approved && (
          <div
            className={`absolute bottom-0 left-0 right-0 transform transition-transform duration-300 ease-in-out ${
              showButtons
                ? 'translate-y-full flex items-center justify-center bg-transparent bg-opacity-50 rounded-md z-10'
                : 'translate-y-0 hidden'
            }`}
          >
            <div className='flex items-center gap-2 p-3 rounded-md w-full'>
              <button
                onClick={() => handleDelete(request.idRequest)}
                className='bg-red-700 text-white px-4 py-2 rounded-md w-full hover:bg-slate-600 hover:shadow-xl hover:shadow-black'
              >
                Xóa
              </button>
            </div>
          </div>
        )}
        {/* form từ chối */}
        {(request.status.toLowerCase() === statusReq.reject ||
          request.status.toLowerCase() === statusReq.pending) && (
          <div
            className={`container absolute bottom-0 left-0 right-0 transform transition-transform duration-300 ease-in-out ${
              showButtons
                ? 'translate-y-full flex items-center justify-center bg-transparent bg-opacity-50 rounded-md z-10'
                : 'translate-y-0 hidden'
            }`}
          >
            {request.reason &&
              request.status.toLowerCase() === statusReq.reject && (
                <div className='border-2 rounded-xl p-2 flex bg-slate-50 w-full'>
                  <div className='text-gray-700 font-medium'>Lý do:</div>
                  <span className='font-semibold ml-2 text-red-500'>
                    {request.reason}
                  </span>
                </div>
              )}
            <div className='flex items-center gap-2 p-3 rounded-md w-full'>
              <button
                onClick={handleOpenPopup}
                className='bg-lime-600 text-white px-4 py-2 rounded-md w-[49%] hover:bg-lime-800  hover:shadow-xl hover:shadow-black'
              >
                Cập nhật
              </button>
              <button
                onClick={() => handleDelete(request.idRequest)}
                className='bg-red-700 text-white px-4 py-2 rounded-md w-[49%] hover:bg-slate-600 hover:shadow-xl hover:shadow-black'
              >
                Xóa
              </button>
            </div>
          </div>
        )}

        {showForm && (
          <FormRequest
            request={request}
            idRequest={request.idRequest}
            onClose={handleCloseForm}
            refetch={refetch}
          />
        )}
      </div>
    </div>
  )
}
