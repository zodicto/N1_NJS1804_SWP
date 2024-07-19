import React, { useContext } from 'react'
import { yupResolver } from '@hookform/resolvers/yup'
import { Controller, useForm } from 'react-hook-form'
import Input from '../../../../../../components/Input'
import { UpdateProfileTT, updateProfileTT } from '../../../../../../utils/rules'
import Button from '../../../../../../components/Button'
import { InputNumber } from 'antd'
import { tutorApi } from '../../../../../../api/tutor.api'
import { useMutation } from '@tanstack/react-query'
import { AppContext } from '../../../../../../context/app.context'
import { toast } from 'react-toastify'

type FormData = UpdateProfileTT
const updateTTSchema = updateProfileTT

interface Props {
  refetch?: (() => void) | undefined
}

export default function UpdateProfile({ refetch }: Props) {
  const { profile } = useContext(AppContext)
  const {
    register,
    handleSubmit,
    control,
    formState: { errors }
  } = useForm<FormData>({
    resolver: yupResolver(updateTTSchema)
  })

  const updateTTMutation = useMutation({
    mutationFn: (formData: FormData) =>
      tutorApi.updateProfileTT(profile?.id as string, formData)
  })

  const onSubmit = (data: FormData) => {
    console.log('Submitted data:', data)

    if (refetch) {
      updateTTMutation.mutate(data, {
        onSuccess: (data) => {
          toast.success(data.data.message)
          refetch()
          console.log(data.data.message)
        },
        onError: (error) => {
          console.error(error.message)
        }
      })
    }
  }

  return (
    <div className='container border-2 h-full rounded-2xl w-full p-3 hover:shadow-xl hover:shadow-black transition-shadow duration-700'>
      <form onSubmit={handleSubmit(onSubmit)}>
        {' '}
        {/* Corrected here */}
        <h2 className='mt-4'>Cập nhật hồ sơ của giảng viên</h2>
        <Input
          name='introduction'
          type='text-area' // Assuming 'text-area' is a custom type for text areas
          placeholder='Giới thiệu'
          className='mt-3'
          classNameInput='h-24 hover:transition duration-700 hover:border-blue-400 p-3 w-[18rem] outline-none border border-gray-300 focus:border-gray-500 focus:shadow-sm rounded-xl'
          register={register}
          classNameError='mt-1 text-red-600 min-h-[1rem] text-sm text-center'
          errorMessage={errors.introduction?.message}
        />
        <Controller
          name='experience'
          control={control}
          render={({ field }) => (
            <InputNumber
              {...field}
              placeholder='Số năm kinh nghiệm'
              className='p-3 w-[18rem] outline-none border border-gray-300 focus:border-gray-500 focus:shadow-sm rounded-xl '
              min={0}
              onChange={(value) => field.onChange(value)}
            />
          )}
        />
        {errors.experience && (
          <div className='text-red-600 mt-1 text-sm'>
            {errors.experience.message}
          </div>
        )}
        <Input
          name='specializedSkill'
          type='text'
          classNameInput='h-14  hover:transition duration-700 hover:border-blue-400 p-3 w-[18rem] outline-none border border-gray-300 focus:border-gray-500 focus:shadow-sm rounded-xl'
          placeholder='Kỹ năng chuyên môn'
          className='mt-3'
          register={register}
          classNameError='mt-1 text-red-600 min-h-[1rem] text-sm text-center'
          errorMessage={errors.specializedSkill?.message}
        />
        <Button
          type='submit'
          className='w-full rounded-xl text-center bg-pink-300 py-3 px-2 uppercase text-white text-sm hover:bg-pink-600 flex justify-center items-center '
        >
          Gửi
        </Button>
      </form>
    </div>
  )
}
