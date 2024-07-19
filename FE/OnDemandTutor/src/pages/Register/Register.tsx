import { faGoogle } from '@fortawesome/free-brands-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { yupResolver } from '@hookform/resolvers/yup'
import { useMutation } from '@tanstack/react-query'
import { omit } from 'lodash'
import { Controller, useForm } from 'react-hook-form'
import { Link, useNavigate } from 'react-router-dom'
import { authApi } from '../../api/auth.api'
import DateSelect from '../../components/DateSelect/DateSelect'
import GenderSelect from '../../components/GenderSelect'
import Input from '../../components/Input'
import { ResReqBody } from '../../types/user.request.type'

import InputNumber from '../../components/InputNumber'
import { Schema, schema } from '../../utils/rules'
import { toast } from 'react-toastify'
import { AppContext } from '../../context/app.context'
import { useContext, useEffect } from 'react'
import { path } from '../../constant/path'
import { HttpStatusCode } from '../../constant/HttpStatusCode.enum'
import { ErrorResponse } from '../../types/utils.type'
import { isAxiosError } from '../../utils/utils'
import { getRefreshTokenFromLS, setRefreshTokenToLS } from '../../utils/auth'
import Button from '../../components/Button'

type FormData = Pick<
  Schema,
  | 'email'
  | 'password'
  | 'gender'
  | 'confirm_password'
  | 'phone'
  | 'date_of_birth'
  | 'fullName'
>

const registerSchema = schema.pick([
  'email',
  'password',
  'fullName',
  'confirm_password',
  'date_of_birth',
  'gender',
  'phone'
])

export default function Register() {
  const navigate = useNavigate()
  const { setIsAuthenticated, setRefreshToken, setProfile } =
    useContext(AppContext)
  const {
    register,
    handleSubmit,
    control,
    setError,
    formState: { errors }
  } = useForm<FormData>({
    defaultValues: {
      fullName: '',
      phone: '',
      gender: 'nam',

      date_of_birth: new Date(1990, 0, 1)
    },
    resolver: yupResolver(registerSchema)
  })

  const registerAccountMutation = useMutation({
    mutationFn: (body: ResReqBody) => authApi.registerAccount(body)
  })

  const onSubmit = handleSubmit((data) => {
    const dateOfBirth = data.date_of_birth
      ? new Date(data.date_of_birth)
      : new Date('1990-01-01')

    const formattedDateOfBirth = `${dateOfBirth.getFullYear()}-${String(
      dateOfBirth.getMonth() + 1
    ).padStart(2, '0')}-${String(dateOfBirth.getDate()).padStart(2, '0')}`

    const body: ResReqBody = {
      ...omit(data, ['confirm_password']),
      date_of_birth: formattedDateOfBirth,
      gender: data.gender ?? 'male'
    }

    console.log(body)

    registerAccountMutation.mutate(body, {
      onSuccess: (data) => {
        console.log(data)

        const refreshToken = getRefreshTokenFromLS()
        console.log(refreshToken)
        setProfile(data.data.data.user)
        setRefreshTokenToLS(refreshToken)
        setIsAuthenticated(true)

        toast.success(data.data.message, {
          icon: false
        })
        navigate(path.home)
      },
      onError: (error) => {
        if (
          isAxiosError<ErrorResponse<any>>(error) &&
          error.response?.status === HttpStatusCode.UnprocessableEntity // 422
        ) {
          const errorAuthen = error.response.data
          console.log(errorAuthen)

          // Hiển thị lỗi trong form
          if (errorAuthen.data) {
            Object.keys(errorAuthen.data).forEach((key) => {
              setError(key as keyof FormData, {
                type: 'server',
                message: errorAuthen.data[key]
              })
            })
          }

          // Hiển thị thông báo lỗi tổng quát
          toast.error(errorAuthen.message)
        } else {
          toast.error('An unexpected error occurred')
        }
      }
    })
  })

  const handleGoogleCallback = (event: {
    origin: string
    data: { profile: any; accessToken: any; refreshToken: any }
  }) => {
    if (event.origin === 'http://localhost:7133') {
      const { profile, accessToken, refreshToken } = event.data
      if (profile && accessToken && refreshToken) {
        setProfile(JSON.parse(profile))
        setIsAuthenticated(true)
        setRefreshToken(refreshToken)

        localStorage.setItem('profile', profile)
        localStorage.setItem('access_token', accessToken)
        localStorage.setItem('refresh_token', refreshToken)
        navigate(path.home)
        toast.success('Đăng nhập thành công')
      } else {
        toast.error('Đăng nhập bại')
      }
    }
  }

  useEffect(() => {
    window.addEventListener('message', handleGoogleCallback)
    return () => {
      window.removeEventListener('message', handleGoogleCallback)
    }
  }, [])

  const signInGoogle = () => {
    const googleAuthWindow = window.open(
      'http://localhost:7133/api/Account/signin-google',
      '_blank',
      'width=500,height=600'
    )

    if (!googleAuthWindow) {
      alert('Popup blocked. Please allow popups for this website.')
    }
  }

  return (
    <div className='py-10 w-[25rem] rounded-2xl border-2 mx-auto my-[2rem] bg-transparent hover:shadow-xl hover:shadow-black'>
      <div className='container mx-auto justify-center flex'>
        <form onSubmit={onSubmit}>
          <div className='text-2xl'>Đăng Ký</div>
          <Input
            name='fullName'
            type='text'
            placeholder='Họ và tên'
            className='mt-8'
            register={register}
            errorMessage={errors.fullName?.message}
          />
          <Input
            name='email'
            type='email'
            placeholder='Email'
            className='mt-2'
            register={register}
            errorMessage={errors.email?.message}
          />
          <Input
            name='password'
            type='password'
            placeholder='Mật khẩu'
            className='mt-0.5 relative'
            register={register}
            errorMessage={errors.password?.message}
            autoComplete='on'
          />
          <Input
            name='confirm_password'
            type='password'
            placeholder='Nhập lại mật khẩu'
            className='mt-2 relative'
            register={register}
            errorMessage={errors.confirm_password?.message}
            autoComplete='on'
          />
          <div className='mt-2 flex flex-wrap flex-col sm:flex-row'>
            <div className='w-full'>
              <Controller
                control={control}
                name='phone'
                render={({ field }) => (
                  <InputNumber
                    className='rounded-3xl w-full'
                    placeholder='Số điện thoại'
                    errorMessage={errors.phone?.message}
                    {...field}
                    onChange={field.onChange}
                  />
                )}
              />
            </div>
          </div>
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
          <div className='mt-3'>
            <Button
              type='submit'
              className='w-full rounded-xl text-center bg-pink-300 py-4 px-2 uppercase text-white text-sm hover:bg-pink-600 flex justify-center items-center'
              isLoading={registerAccountMutation.isPending}
              disabled={registerAccountMutation.isPending}
            >
              Đăng Ký
            </Button>
          </div>
        </form>
      </div>
      <div>
        <div>
          <span>---------------------------</span>
          <span>hoặc</span>
          <span>---------------------------</span>
        </div>
        <div className='justify-center flex py-2'>
          <button
            onClick={signInGoogle}
            className='bg-black text-white border-black border-2 w-[300px] rounded-lg justify-center items-center flex py-2 shadow-2xl hover:bg-white hover:text-black'
          >
            <div className='pr-2'>
              <FontAwesomeIcon icon={faGoogle} />
            </div>
            <div>Google</div>
          </button>
        </div>
        <div className='my-4'>
          <div>
            <span className='text-gray-600 mr-1'>Bạn đã có tài khoản?</span>
            <Link
              className='text-gray-500 underline hover:text-red-500'
              to='/Login'
            >
              Đăng Nhập
            </Link>
          </div>
        </div>
      </div>
    </div>
  )
}
