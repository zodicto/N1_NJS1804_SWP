import { yupResolver } from '@hookform/resolvers/yup'
import { useMutation, useQuery } from '@tanstack/react-query'
import { getDownloadURL, ref, uploadBytes } from 'firebase/storage'
import { useContext, useEffect, useMemo, useRef, useState } from 'react'
import { Controller, useForm } from 'react-hook-form'
import { toast } from 'react-toastify'
import { v4 as uuidv4 } from 'uuid'
import userApi from '../../../../api/user.api'
import userImage from '../../../../assets/img/user.svg'
import Button from '../../../../components/Button'
import DateSelect from '../../../../components/DateSelect/DateSelect'
import GenderSelect from '../../../../components/GenderSelect'
import Input from '../../../../components/Input'
import InputFile from '../../../../components/InputFile'
import InputNumber from '../../../../components/InputNumber'
import { AppContext } from '../../../../context/app.context'
import { setProfileToLS } from '../../../../utils/auth'
import { storage } from '../../../../utils/firebase'
import { UpdateSchema, updateSchema } from '../../../../utils/rules'

import { UpdateProfileBody } from '../../../../types/user.request.type'
import { reset } from 'numeral'

type FormData = Pick<
  UpdateSchema,
  | 'fullName'
  | 'phone'
  | 'date_of_birth'
  | 'address'
  | 'gender'
  | 'avatar'
  | 'roles'
>

const profileSchema = updateSchema.pick([
  'fullName',
  'address',
  'phone',
  'date_of_birth',
  'gender',
  'avatar',
  'roles'
])

export default function Profile() {
  const { profile } = useContext(AppContext)

  const [urlImage, setUrlImage] = useState<string | null>(null)

  const {
    handleSubmit,
    watch,
    setValue,
    register,
    control,
    reset,
    formState: { errors }
  } = useForm<FormData>({
    defaultValues: {
      fullName: '',
      phone: '',
      gender: 'nam',
      avatar: '',
      date_of_birth: new Date(1990, 0, 1)
    },
    resolver: yupResolver(profileSchema)
  })
  const { setProfile } = useContext(AppContext)

  const [file, setFile] = useState<File | null>(null)
  const fileInputRef = useRef<HTMLInputElement | null>(null)

  const previewImage = useMemo(() => {
    return file ? URL.createObjectURL(file) : ''
  }, [file])

  const { data: ProfileData, refetch } = useQuery({
    queryKey: ['Account'],
    queryFn: () => userApi.getProfile(profile?.id as string)
  })

  console.log(ProfileData)

  const profileAPI = ProfileData?.data.data
  console.log(profile)

  useEffect(() => {
    if (profileAPI) {
      refetch()
      setValue('fullName', profileAPI.fullName || '')
      setValue('avatar', profileAPI.avatar || '')
      setValue('phone', profileAPI.phone || '')
      setValue('address', profileAPI.address || '')
      setValue('gender', profileAPI.gender || 'nam')
      setValue(
        'date_of_birth',
        profileAPI.date_of_birth
          ? new Date(profileAPI.date_of_birth)
          : new Date(1990, 0, 1)
      )
      setValue('phone', profileAPI.phone || ''),
        setValue('roles', profileAPI.roles)
    }
  }, [profileAPI, setValue])

  console.log(ProfileData)
  console.log(profileAPI)

  const updateProfileMutation = useMutation({
    mutationFn: userApi.updateProfile
  })

  const uploadAvatar = async (file: File): Promise<string> => {
    const imageRef = ref(storage, `avatarUser/${file.name + uuidv4()}`)
    const snapshot = await uploadBytes(imageRef, file)
    const url = await getDownloadURL(snapshot.ref)
    return url
  }

  function convertDateOfBirth(date_of_birth: string): string {
    const dateOfBirth = date_of_birth
      ? new Date(date_of_birth)
      : new Date('1990-01-01')

    return `${dateOfBirth.getFullYear()}-${String(
      dateOfBirth.getMonth() + 1
    ).padStart(2, '0')}-${String(dateOfBirth.getDate()).padStart(2, '0')}`
  }

  const onSubmit = handleSubmit(async (data: FormData) => {
    console.log(data)

    try {
      if (file) {
        const url = await uploadAvatar(file)

        setUrlImage(url)

        setValue('avatar', url) // Update the avatar URL in form data
        setFile(null)
        if (fileInputRef.current) {
          fileInputRef.current.value = ''
        }
      }

      const formData: UpdateProfileBody = {
        fullName:
          data.fullName !== profileAPI?.fullName
            ? data.fullName
            : profileAPI?.fullName || '',
        phone:
          data.phone !== profileAPI?.phone
            ? data.phone!
            : profileAPI?.phone || '',
        address:
          data.address !== profileAPI?.address
            ? data.address || ''
            : profileAPI?.address || '',
        gender:
          data.gender !== profileAPI?.gender
            ? data.gender || ''
            : profileAPI?.gender || 'nam',
        date_of_birth:
          convertDateOfBirth(data.date_of_birth?.toString() || '') !==
          profileAPI?.date_of_birth
            ? convertDateOfBirth(data.date_of_birth?.toString() || '')
            : profileAPI?.date_of_birth || '',
        avatar: file ? await uploadAvatar(file) : profileAPI?.avatar || '',
        roles: profileAPI?.roles || ''
      }

      const updateRes = await updateProfileMutation.mutateAsync(formData, {
        onSuccess: (data) => {
          toast.success(data.data.message)
          setProfile(data.data.data)
          setProfileToLS(data.data.data)
          refetch()
          reset()
        },
        onError: (error) => {
          toast.error(error.message)
          console.error(error)
        }
      })

      console.log(updateRes)
    } catch (error) {
      console.log(error)
    }
  })

  const handleChangeFile = (file?: File) => {
    setFile(file || null)
  }

  return (
    <div className='pb-10 rounded-sm bg-transparent px-2 shadow-md:px-7 md:pb-20 shadow-black'>
      <div className='border-b border-b-gray py-6'>
        <h1 className='text-lg font-medium capitalize text-gray-900'>
          Hồ Sơ của tôi
        </h1>
        <div className='mt-1 text-sm text-gray-700'>
          Quản lý thông tin hồ sơ để bảo vệ tài khoản
        </div>
      </div>

      <form
        className='mt-8 flex flex-col-reverse md:flex-row md:items-start'
        onSubmit={onSubmit}
      >
        <div className='mt-6 flex-grow md:mt-0'>
          {/* email */}
          <div className='flex flex-wrap flex-col sm:flex-row'>
            <div className='sm:w-[20%] truncate pt-3 sm:text-right capitalize'>
              Email
            </div>
            <div className='sm:w-[80%] sm:pl-5'>
              <div className='pt-3 text-gray-700 sm:text-left ml-3'>
                {profile?.email}
              </div>
            </div>
          </div>

          <div className='mt-6 flex flex-wrap flex-col sm:flex-row'>
            <div className='sm:w-[20%] truncate pt-3 sm:text-right capitalize'>
              Tên
            </div>
            <div className='sm:w-[80%] sm:pl-5'>
              <Input
                className='px-3 py-auto w-full focus:border-gray-500 focus:shadow-sm rounded-xl my-auto'
                classNameInput='rounded-xl border-2 w-full h-10 text-left hover:shadow-black hover:shadow-sm pl-2'
                name='fullName'
                register={register}
                placeholder='Họ và Tên'
                errorMessage={errors.fullName?.message}
              />
            </div>
          </div>

          <div className='mt-2 flex flex-wrap flex-col sm:flex-row'>
            <div className='sm:w-[20%] truncate pt-3 sm:text-right capitalize'>
              Địa chỉ
            </div>
            <div className='sm:w-[80%] sm:pl-5'>
              <Input
                className='px-3 py-auto w-full focus:border-gray-500 focus:shadow-sm rounded-xl my-auto'
                classNameInput='rounded-xl border-2 w-full h-10 text-left hover:shadow-black hover:shadow-sm pl-2'
                name='address'
                register={register}
                placeholder='Địa chỉ'
                errorMessage={errors.address?.message}
              />
            </div>
          </div>

          {/* số điện thoại */}
          <div className='mt-2 flex flex-wrap flex-col sm:flex-row'>
            <div className='sm:w-[20%] truncate sm:text-right capitalize'>
              Số điện thoại
            </div>
            <div className='sm:w-[80%] sm:pl-5'>
              <Controller
                control={control}
                name='phone'
                render={({ field }) => (
                  <InputNumber
                    className='px-3 w-full outline-none focus:border-gray-500 focus:shadow-sm rounded-sm'
                    placeholder='Số điện thoại'
                    classNameInput='rounded-xl border-2 w-full h-10 text-left pl-2 hover:shadow-black hover:shadow-sm'
                    errorMessage={errors.phone?.message}
                    {...field}
                    onChange={field.onChange}
                    value={field.value}
                  />
                )}
              />
            </div>
          </div>

          {/*  ngày sinh */}
          <Controller
            control={control}
            name='date_of_birth'
            render={({ field }) => (
              <DateSelect
                name='Ngày sinh'
                errorMessage={errors.date_of_birth?.message}
                onChange={field.onChange}
                value={field.value}
              />
            )}
          />

          {/*  giới tính */}
          <Controller
            control={control}
            name='gender'
            render={({ field }) => (
              <GenderSelect
                errorMessage={errors.gender?.message}
                onChange={field.onChange}
                value={field.value || 'nam'}
              />
            )}
          />

          <div className='mt-5  '>
            <Button
              className='mx-auto flex h-9 justify-center items-center w-[5rem] bg-pink-400 border-2 rounded-xl px-5 text-sm text-white hover:bg-black'
              type='submit'
            >
              <span>Lưu</span>
            </Button>
          </div>
        </div>

        <div className='flex justify-center md:w-72 md:border-l md:border-l-gray-200'>
          <div className='flex flex-col items-center'>
            <div className='my-5 h-24 w-24'>
              {/* ảnh đại diện */}
              <img
                src={
                  file
                    ? previewImage
                    : profileAPI?.avatar
                    ? profileAPI.avatar
                    : userImage
                }
                className='h-full w-full rounded-full object-cover'
              />
            </div>
            <InputFile onChange={handleChangeFile} />

            <div className='mt-3 text-gray-400'>
              <div>Dung lượng file tối đa 1 MB</div>
              <div>Định dạng: .JPEG, .PNG</div>
            </div>
          </div>
        </div>
      </form>
    </div>
  )
}
