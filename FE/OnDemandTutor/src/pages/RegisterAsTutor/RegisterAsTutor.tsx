import React, { useRef, useState, useContext } from 'react'
import { useMutation } from '@tanstack/react-query'
import { getDownloadURL, ref, uploadBytes } from 'firebase/storage'
import { toast } from 'react-toastify'
import { v4 as uuidv4 } from 'uuid'
import { studentApi } from '../../api/student.api'
import InputFile from '../../components/InputFile'
import { RequestTutorBody } from '../../types/user.request.type'
import { storage } from '../../utils/firebase'
import { AppContext } from '../../context/app.context'
import TextArea from 'antd/es/input/TextArea'
import { useLocation } from 'react-router-dom'

interface Props {
  ReSignUp?: string
  refetch?: (() => void) | undefined
  isVisible?: boolean
}

export default function RegisterAsTutor({
  ReSignUp,
  refetch,
  isVisible = true
}: Props) {
  const [file, setFile] = useState<File | null>(null)
  const [experience, setExperience] = useState<number | null>(null)
  const [imageQualification, setImageQualification] = useState<string>('')
  const [introduction, setIntroduction] = useState<string>('')
  const [qualifiCationName, setQualificationName] = useState<string>('')
  const [specializedSkills, setSpecializedSkills] = useState<string>('')
  const [subject, setSubject] = useState<string>('')
  const [type, setType] = useState<string>('')
  const [previewUrl, setPreviewUrl] = useState<string | null>(null)
  const [imageError, setImageError] = useState<string>('')

  const fileInputRef = useRef<HTMLInputElement | null>(null)

  const { state } = useLocation()

  const { profile } = useContext(AppContext)

  const ReqMutation = useMutation({
    mutationFn: (body: RequestTutorBody) =>
      studentApi.registerAsTutor(body, profile?.id as string),
    onSuccess: () => {
      toast.success('Đăng ký thành công')
      resetForm()
    },
    onError: (error) => {
      toast.error(error.message)
    }
  })

  const ReSignUpMutation = useMutation({
    mutationFn: (body: RequestTutorBody) =>
      studentApi.reSignUpofTutor(body, profile?.id as string),
    onSuccess: (data) => {
      toast.success(data.data.message)
      resetForm()
    },
    onError: (error) => {
      toast.error(error.message)
    }
  })

  const mutation = ReSignUp ? ReSignUpMutation : ReqMutation

  const handleChangeFile = (file?: File) => {
    setFile(file || null)
    if (file) {
      const reader = new FileReader()
      reader.onloadend = () => {
        setPreviewUrl(reader.result as string)
      }
      reader.readAsDataURL(file)
    } else {
      setPreviewUrl(null)
    }
  }

  const uploadAvatar = async (file: File): Promise<string> => {
    const imageRef = ref(storage, `certificate/${file.name + uuidv4()}`)
    const snapshot = await uploadBytes(imageRef, file)
    const url = await getDownloadURL(snapshot.ref)
    return url
  }

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()

    if (
      !experience ||
      !introduction ||
      !qualifiCationName ||
      !specializedSkills ||
      !subject ||
      !type
    ) {
      toast.error('Vui lòng điền đầy đủ thông tin bắt buộc')
      return
    }

    if (!file && !imageQualification) {
      setImageError('Ảnh là bắt buộc')
      return
    }

    try {
      let imageUrl = imageQualification
      if (file) {
        imageUrl = await uploadAvatar(file)
        setImageQualification(imageUrl)
        setFile(null)
        if (fileInputRef.current) {
          fileInputRef.current.value = ''
        }
      }

      const formData: RequestTutorBody = {
        experience: experience!,
        imageQualification: imageUrl,
        introduction,
        qualifiCationName,
        specializedSkills,
        subject,
        type
      }

      console.log(formData)

      mutation.mutate(formData, {
        onSuccess: (data) => {
          toast.success(data.data.message)
          resetForm()
          if (refetch) {
            refetch()
          }
        },
        onError: (error) => {
          toast.error(error.message)
        }
      })
    } catch (error) {
      console.log(error)
    }
  }

  const resetForm = () => {
    setExperience(null)
    setImageQualification('')
    setIntroduction('')
    setQualificationName('')
    setSpecializedSkills('')
    setSubject('')
    setType('')
    setPreviewUrl(null)
    setImageError('')
  }

  return (
    <div className='flex items-center justify-center'>
      <div className='p-6'>
        <form
          onSubmit={handleSubmit}
          className='w-[50rem] space-y-4 border-2 rounded-xl p-4 bg-white shadow-black shadow-lg'
        >
          <div>
            <h1 className='text-lg text-red-500'>
              {ReSignUp ? 'Chỉnh sửa đơn Gia Sư' : 'Đăng ký gia sư'}
            </h1>
          </div>

          <div className='space-y-2'>
            <label className='block mb-2'>
              Giới thiệu :
              <TextArea
                value={introduction}
                onChange={(e) => setIntroduction(e.target.value)}
                className='w-full p-2 border rounded'
                required
              />
            </label>

            <label className='block mb-2'>
              Số năm kinh nghiệm (số):
              <input
                type='number'
                value={experience || ''}
                onChange={(e) => setExperience(Number(e.target.value))}
                className='w-full p-2 border rounded'
                required
              />
            </label>

            <label className='block mb-2'>
              Môn học:
              <select
                value={subject}
                onChange={(e) => setSubject(e.target.value)}
                className='w-full p-2 border rounded'
                required
              >
                <option value=''>Chọn môn học</option>
                <option value='Ngữ văn'>Ngữ văn</option>
                <option value='Toán học'>Toán học</option>
                <option value='Vật lý'>Vật lý</option>
                <option value='Hóa học'>Hóa học</option>
                <option value='Sinh học'>Sinh học</option>
                <option value='Lịch sử'>Lịch sử</option>
                <option value='Địa lý'>Địa lý</option>
                <option value='Giáo dục công dân'>Giáo dục công dân</option>
                <option value='Ngoại ngữ'>Ngoại ngữ</option>
                <option value='Tin học'>Tin học</option>
              </select>
            </label>

            <label className='block mb-2'>
              Kĩ năng đặc biệt:
              <TextArea
                value={specializedSkills}
                onChange={(e) => setSpecializedSkills(e.target.value)}
                className='w-full p-2 border rounded'
                required
              />
            </label>

            <label className='block mb-2'>
              Tên bằng cắp (chứng chỉ):
              <input
                type='text'
                value={qualifiCationName}
                onChange={(e) => setQualificationName(e.target.value)}
                className='w-full p-2 border rounded'
                required
              />
            </label>

            <label className='block mb-2'>
              Loại :
              <select
                value={type}
                onChange={(e) => setType(e.target.value)}
                className='w-full p-2 border rounded'
                required
              >
                <option value=''>-- Chọn loại --</option>
                <option value='Chứng chỉ'>Chứng chỉ</option>
                <option value='Bằng Cấp'>Bằng Cấp</option>
              </select>
            </label>

            <div className='mb-2 justify-center '>
              <label htmlFor=''>Hãy chọn ảnh chứng chỉ</label>
              <div className='justify-center mt-2   flex'>
                <InputFile onChange={handleChangeFile} />
                {imageError && <p className='text-red-500'>{imageError}</p>}
              </div>
            </div>
            <div className='mb-2 justify-center '>
              {previewUrl && (
                <img
                  src={previewUrl}
                  alt='Ảnh chứng chỉ'
                  className='mt-4 mx-auto'
                />
              )}
            </div>
          </div>
          <div className='mt-6 flex justify-around'>
            <div className='w-[49%]'>
              <button
                type='submit'
                className='w-full p-3 bg-pink-400 text-white rounded-lg hover:bg-pink-500'
              >
                {ReSignUp ? 'Chỉnh sửa đơn gia sư' : 'Đăng ký'}
              </button>
            </div>
            <div className='w-[49%]'>
              <button
                onClick={resetForm}
                type='button'
                className='w-full p-3 bg-black text-white rounded-lg hover:bg-red-500'
              >
                Xóa tất cả
              </button>
            </div>
          </div>
        </form>
      </div>
    </div>
  )
}
