import { yupResolver } from '@hookform/resolvers/yup'
import { useForm } from 'react-hook-form'
import Button from '../../../../components/Button'
import Input from '../../../../components/Input'
import { UserSchema, userSchema } from '../../../../utils/rules'
import { useMutation } from '@tanstack/react-query'
import userApi from '../../../../api/user.api'
import { omit } from 'lodash'

import { toast } from 'react-toastify'
import { ChangePasswordReqBody } from '../../../../types/user.request.type'

type FormData = Pick<
  UserSchema,
  'password' | 'new_password' | 'confirm_password'
>
const passwordSchema = userSchema.pick([
  'password',
  'new_password',
  'confirm_password'
])

export default function ChangePassword() {
  //  đây là 1 kĩ thuật dùng form contexxt

  const {
    register,
    setError,
    formState: { errors },
    reset,
    handleSubmit
  } = useForm<FormData>({
    defaultValues: {
      password: '',
      new_password: '',
      confirm_password: ''
    },
    resolver: yupResolver(passwordSchema)
  })

  const changePasswordMutation = useMutation({
    mutationFn: userApi.changePassword
  })

  const onSubmit = handleSubmit(async (data) => {
    const body: ChangePasswordReqBody = omit(data, [
      'confirm_password'
    ]) as ChangePasswordReqBody

    try {
      const res = await changePasswordMutation.mutateAsync(body, {
        onSuccess: () => {
          toast.success('Đổi mật khẩu thành công')
        }
      })

      reset()
    } catch (error) {
      console.error(error)
    }
  })

  return (
    <div className=' w-[60rem] rounded-sm bg-transparent pb-10 shadow md:px-7 md:pb-20 mx-2 pl-[50rem]'>
      <div className='border-b border-b-gray-200 py-6'>
        <h1 className='text-lg font-medium capitalize text-gray-900'>
          Đổi mật khẩu
        </h1>
        <div className='mt-1 text-sm text-gray-700'>
          Quản lý thông tin hồ sơ để bảo mật tài khoản
        </div>
      </div>
      <form className='mt-8 mx-auto max-w-2xl' onSubmit={onSubmit}>
        <div className='mt-6 flex-grow md:mt-0 md:pr-12'>
          <div className='mt-2 flex flex-col flex-wrap sm:flex-row'>
            <div className='truncate pt-3 capitalize sm:w-[20%] sm:text-right'>
              Mật khẩu cũ
            </div>
            <div className='sm:w-[80%] sm:pl-5'>
              <Input
                classNameInput='rounded-xl border-2 w-full h-10 text-left pl-2 hover:shadow-black hover:shadow-sm'
                className='relative '
                register={register}
                name='password'
                type='password'
                placeholder='Mật khẩu cũ'
                errorMessage={errors.password?.message}
              />
            </div>
          </div>
          <div className='mt-2 flex flex-col flex-wrap sm:flex-row'>
            <div className='truncate pt-3 capitalize sm:w-[20%] sm:text-right'>
              Mật khẩu mới
            </div>
            <div className='sm:w-[80%] sm:pl-5'>
              <Input
                classNameInput='rounded-xl border-2 w-full h-10 text-left pl-2 hover:shadow-black hover:shadow-sm'
                className='relative '
                register={register}
                name='new_password'
                type='password'
                placeholder='Mật khẩu mới'
                errorMessage={errors.new_password?.message}
              />
            </div>
          </div>
          <div className='mt-2 flex'>
            <div className='ml-5 pt-3 pr-4 capitalize sm:w-[26%] sm:text-left'>
              Nhập lại mật khẩu
            </div>
            <div className='sm:w-[74%] '>
              <Input
                classNameInput='rounded-xl border-2 w-full h-10 text-left pl-2 hover:shadow-black hover:shadow-sm'
                className='relative'
                register={register}
                name='confirm_password'
                type='password'
                placeholder='Nhập lại mật khẩu'
                errorMessage={errors.confirm_password?.message}
              />
            </div>
          </div>

          <div className='mt-2 flex flex-col flex-wrap sm:flex-row  '>
            <div className='truncate pt-3 capitalize sm:w-[20%] sm:text-right' />
            <div className='sm:w-[80%] sm:pl-5 flex items-center justify-center  '>
              <Button
                className=' flex h-9 items-center w-[5rem] bg-pink-400 border-2 rounded-xl  px-5  text-sm text-white hover:bg-black'
                type='submit'
              >
                Lưu
              </Button>
            </div>
          </div>
        </div>
      </form>
    </div>
  )
}
