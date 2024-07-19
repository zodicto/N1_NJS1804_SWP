import React, { useContext, useState } from 'react'
import Button from '../../../../../../components/Button'
import InputFile from '../../../../../../components/InputFile'
import { getDownloadURL, ref, uploadBytes } from 'firebase/storage'
import { storage } from '../../../../../../utils/firebase'
import { v4 as uuidv4 } from 'uuid'
import { tutorApi } from '../../../../../../api/tutor.api'
import { AppContext } from '../../../../../../context/app.context'
import { toast } from 'react-toastify'

type FormData = {
  subjects: string
  name: string
  type: string
  img: string
}

interface Props {
  refetch?: () => void
}

export default function UpdateMajorTT({ refetch }: Props) {
  const { profile } = useContext(AppContext)
  const [formData, setFormData] = useState<FormData>({
    subjects: '',
    name: '',
    type: '',
    img: ''
  })

  const [file, setFile] = useState<File | null>(null)
  const [previewImage, setPreviewImage] = useState<string | undefined>(
    undefined
  )
  const [errors, setErrors] = useState<{ [key: string]: string | undefined }>(
    {}
  )

  const uploadAvatar = async (file: File): Promise<string> => {
    const imageRef = ref(storage, `certificate/${file.name + uuidv4()}`)
    const snapshot = await uploadBytes(imageRef, file)
    const url = await getDownloadURL(snapshot.ref)
    return url
  }

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target
    setFormData({
      ...formData,
      [name]: value
    })
  }

  const handleChangeFile = async (file?: File) => {
    if (file) {
      try {
        const url = await uploadAvatar(file)
        setFile(file)
        setPreviewImage(url)
        setFormData((prevFormData) => ({
          ...prevFormData,
          img: url
        }))
      } catch (error) {
        console.error('Error uploading file:', error)
      }
    } else {
      setFile(null)
      setPreviewImage(undefined)
      setFormData((prevFormData) => ({
        ...prevFormData,
        img: ''
      }))
    }
  }

  const onSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault()
    if (refetch) {
      try {
        // Example usage: add subject
        if (formData.subjects && profile?.id) {
          await tutorApi
            .addSubject(profile.id, formData.subjects)
            .then((response) => {
              toast.success(response.message)
              refetch()
            })
            .catch((error) => {
              console.error('Error:', error)
              toast.error('Failed to add subject')
            })
        }

        const { name, type, img } = formData
        if (name && type && profile?.id) {
          await tutorApi
            .addQualification(profile.id, {
              name: name,
              type: type,
              img: img
            })
            .then((response) => {
              toast.success(response.message)
              refetch()
            })
            .catch((error) => {
              console.error('Failed to add qualification:', error)
              toast.error('Failed to add qualification')
            })
        }

        console.log('Submitted data:', formData)
      } catch (error) {
        console.error('Submit error:', error)
        toast.error('Failed to submit form')
      }
    }
  }

  return (
    <div className='container border-2 h-full rounded-2xl min:w-[18rem] p-3 hover:shadow-xl hover:shadow-black transition-shadow duration-700'>
      <form onSubmit={onSubmit}>
        <h2 className='mt-4'>Cập nhật môn học của giảng viên</h2>

        <div className='flex flex-col mt-5'>
          <select
            name='subjects'
            value={formData.subjects}
            onChange={handleChange}
            className='h-14 hover:transition duration-700 w-full hover:border-blue-400 p-3 outline-none border border-gray-300 focus:border-gray-500 focus:shadow-sm rounded-xl'
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
          {errors.subjects && (
            <p className='text-red-600 mt-1 text-[0.75rem]'>
              {errors.subjects}
            </p>
          )}
        </div>

        <input
          name='name'
          value={formData.name}
          onChange={handleChange}
          type='text'
          className=' mt-3 min-w-[18rem] h-14 hover:transition duration-700 hover:border-blue-400 p-3 w-full outline-none border border-gray-300 focus:border-gray-500 focus:shadow-sm rounded-xl'
          placeholder='Tên bằng cắp/ chứng chỉ'
        />

        <label className='block my-3'>
          <select
            name='type'
            value={formData.type}
            onChange={handleChange}
            className='min-w-[18rem]   h-14 hover:transition duration-700 w-full hover:border-blue-400 p-3 outline-none border border-gray-300 focus:border-gray-500 focus:shadow-sm rounded-xl'
          >
            <option value=''>-- Chọn loại --</option>
            <option value='Chứng chỉ'>Chứng chỉ</option>
            <option value='Bằng Cấp'>Bằng Cấp</option>
          </select>
          {errors.type && (
            <p className='text-red-600 mt-1 text-[0.75rem]'>{errors.type}</p>
          )}
        </label>

        <div className='min-w-[18rem] mx-auto flex items-center justify-center px-4 py-4'>
          <InputFile onChange={handleChangeFile} />
        </div>

        {previewImage && (
          <img src={previewImage} className='w-full h-full' alt='Preview' />
        )}
        <Button
          type='submit'
          className='w-full rounded-xl text-center bg-pink-300 py-3 px-2 uppercase text-white text-sm hover:bg-pink-600 flex justify-center items-center'
        >
          Gửi
        </Button>
      </form>
    </div>
  )
}
