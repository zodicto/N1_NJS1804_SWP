import { useMutation } from '@tanstack/react-query'
import React, { useContext, useState } from 'react'
import { toast } from 'react-toastify'
import { studentApi } from '../../../api/student.api'
import { AppContext } from '../../../context/app.context'

interface FormRequestProps {
  onClose: () => void
  idRequest?: string
  refetch?: () => void | undefined
  idAccountTutor: string
}

interface FormType {
  idUser: string
  description: string
  idAccountTutor: string
}

export default function Report({
  onClose,

  idAccountTutor
}: FormRequestProps) {
  const { profile } = useContext(AppContext)
  const [reportContent, setReportContent] = useState('')
  const [showConfirmation, setShowConfirmation] = useState<boolean>(false)
  const [formSubmitted, setFormSubmitted] = useState<boolean>(false)

  const reportMutation = useMutation({
    mutationFn: (body: FormType) => studentApi.createComplaint(body)
  })

  const handleSubmit = (event: any) => {
    event.preventDefault()

    console.log(reportContent)

    if (!reportContent.trim()) {
      alert('Nội dung báo cáo không được để trống')
      return
    }

    // Xử lý logic khi form được nộp và nội dung hợp lệ
    console.log('Submitted:', reportContent)

    const res: FormType = {
      idUser: profile?.id as string,
      idAccountTutor: idAccountTutor,
      description: reportContent
    }
    console.log(res)

    reportMutation.mutate(res, {
      onSuccess: (data) => {
        toast.success(data.data.message)
      },
      onError: (error) => toast.error(error.message)
    })

    // Reset input sau khi nộp form
    setReportContent('')
    // Đánh dấu là form đã được nộp thành công
    setFormSubmitted(true)
  }

  const handleChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    setReportContent(event.target.value)
  }

  const handleConfirmCancel = () => {
    setShowConfirmation(false)
    onClose()
  }

  const handleResetForm = () => {
    setFormSubmitted(false)
  }

  return (
    <div className='w-full'>
      <form onSubmit={handleSubmit}>
        <textarea
          value={reportContent}
          onChange={handleChange}
          placeholder='Nhập nội dung báo cáo...'
          className='border border-gray-300 p-2 rounded-md w-full'
          rows={5}
          required
        />

        <div className='col-span-2 flex justify-between'>
          <div className='w-[49%]'>
            <button
              type='submit'
              className='w-full p-3 bg-pink-500 text-white rounded-lg hover:bg-pink-300 focus:outline-none'
            >
              Nộp
            </button>
          </div>
          <div className='w-[49%]'>
            <button
              type='button'
              onClick={() => setShowConfirmation(true)}
              className='w-full p-3 bg-black text-white rounded-lg hover:bg-gray-500 hover:shadow-lg focus:outline-none'
            >
              Hủy
            </button>
          </div>
        </div>
      </form>

      {showConfirmation && (
        <div className='fixed top-0 left-0 w-full h-full bg-black bg-opacity-50 flex justify-center items-center'>
          <div className='bg-white p-4 rounded-lg'>
            <p className='text-lg mb-4'>Bạn có chắc chắn muốn hủy không?</p>
            <div className='flex justify-end'>
              <button
                className='px-4 py-2 bg-red-500 text-white rounded-lg mr-2 hover:bg-red-600'
                role='submit'
                onClick={() => {
                  setShowConfirmation(false)
                }}
              >
                Hủy
              </button>
              <button
                className='px-4 py-2 bg-green-500 text-white rounded-lg hover:bg-green-600'
                type='button'
                onClick={() => {
                  handleConfirmCancel()
                  handleResetForm()
                }}
              >
                Đồng ý
              </button>
            </div>
          </div>
        </div>
      )}

      {formSubmitted && (
        <div className='fixed top-0 left-0 w-full h-full bg-black bg-opacity-50 flex justify-center items-center'>
          <div className='bg-white p-4 rounded-lg'>
            <p className='text-lg mb-4'>Form đã được nộp thành công!</p>
            <button
              className='px-4 py-2 bg-pink-500 text-white rounded-lg hover:bg-green-500'
              onClick={handleResetForm}
            >
              Nhập lại
            </button>
          </div>
        </div>
      )}
    </div>
  )
}
