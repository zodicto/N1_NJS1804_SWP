import { faGoogle } from '@fortawesome/free-brands-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { yupResolver } from '@hookform/resolvers/yup'
import { useMutation } from '@tanstack/react-query'
import { useContext, useEffect, useState } from 'react'
import { useForm } from 'react-hook-form'
import { Link, useLocation, useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import { authApi } from '../../api/auth.api'
import userApi from '../../api/user.api'
import Button from '../../components/Button'
import Input from '../../components/Input'
import { HttpStatusCode } from '../../constant/HttpStatusCode.enum'
import { path } from '../../constant/path'
import { AppContext } from '../../context/app.context'
import {
  ForgotPasswordReqBody,
  LoginReqBody
} from '../../types/user.request.type'
import { ErrorResponse } from '../../types/utils.type'
import { getRefreshTokenFromLS } from '../../utils/auth'
import { Schema, schema } from '../../utils/rules'
import { isAxiosError } from '../../utils/utils'

type FormData = Pick<Schema, 'email' | 'password'>
const loginSchema = schema.pick(['email', 'password'])
const forgotPasswordSchema = schema.pick(['email'])

export default function Login() {
  const { setIsAuthenticated, setRefreshToken, setProfile } =
    useContext(AppContext)
  const navigate = useNavigate()

  const [showForgotPassword, setShowForgotPassword] = useState(false)

  const {
    register: loginRegister,
    handleSubmit: handleLoginSubmit,
    setError: setLoginError,
    formState: { errors: loginErrors },
    reset: resetLoginForm
  } = useForm<FormData>({
    resolver: yupResolver(loginSchema)
  })

  const {
    register: forgotPasswordRegister,
    handleSubmit: handleForgotPasswordSubmit,
    formState: { errors: forgotPasswordErrors },
    reset: resetForgotPasswordForm
  } = useForm<Pick<FormData, 'email'>>({
    resolver: yupResolver(forgotPasswordSchema)
  })

  const forgotPasswordMutation = useMutation({
    mutationFn: (body: ForgotPasswordReqBody) => userApi.forgotPassword(body)
  })

  const onSubmitForgotPassword = (data: Pick<FormData, 'email'>) => {
    console.log('email', data)

    forgotPasswordMutation.mutate(data, {
      onSuccess: () => {
        toast.success('Đã gửi')
        setShowForgotPassword(false)
        resetForgotPasswordForm()
      },
      onError: (error) => {
        console.log(error)
      }
    })
  }

  const loginMutation = useMutation({
    mutationFn: (body: LoginReqBody) => authApi.loginAccount(body)
  })

  const onSubmitLogin = (data: FormData) => {
    console.log('login', data)

    loginMutation.mutate(data, {
      onSuccess: (data) => {
        console.log('login', data)

        const refreshToken = getRefreshTokenFromLS()
        console.log(refreshToken)

        setRefreshToken(refreshToken)
        setIsAuthenticated(true)
        setProfile(data.data.data.user)
        toast.success(data.data.message)
        navigate(path.home)
      },
      onError: (error) => {
        if (
          isAxiosError<ErrorResponse<any>>(error) &&
          error.response?.status === HttpStatusCode.UnprocessableEntity
        ) {
          const errorAuthen = error.response.data
          if (errorAuthen.data) {
            Object.keys(errorAuthen.data).forEach((key) => {
              setLoginError(key as keyof FormData, {
                type: 'server',
                message: errorAuthen.data[key]
              })
            })
          }
          toast.error(errorAuthen.message)
        } else {
          toast.error('An unexpected error occurred')
        }
      }
    })
  }

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
      <div className='container justify-center flex'>
        {!showForgotPassword ? (
          <form onSubmit={handleLoginSubmit(onSubmitLogin)} action='submit'>
            <div className='text-2xl'>Đăng Nhập</div>
            <Input
              name='email'
              type='email'
              placeholder='Email'
              className='mt-8'
              register={loginRegister}
              errorMessage={loginErrors.email?.message}
            />
            <Input
              name='password'
              type='password'
              placeholder='Mật khẩu'
              className='relative'
              register={loginRegister}
              errorMessage={loginErrors.password?.message}
              autoComplete='on'
            />
            <div className='mt-3'>
              <Button
                type='submit'
                className='w-full rounded-xl text-center bg-pink-300 py-4 px-2 uppercase text-white text-sm hover:bg-pink-600 flex justify-center items-center'
                isLoading={loginMutation.isPending}
                disabled={loginMutation.isPending}
              >
                Đăng Nhập
              </Button>
            </div>
            <div className='mt-3'>
              <button
                type='button'
                className='w-full text-center text-blue-500'
                onClick={() => setShowForgotPassword(true)}
              >
                Quên mật khẩu?
              </button>
            </div>
          </form>
        ) : (
          <form
            onSubmit={handleForgotPasswordSubmit(onSubmitForgotPassword)}
            action='submit'
          >
            <div className='text-2xl'>Quên mật khẩu</div>
            <Input
              name='email'
              type='email'
              placeholder='Nhập email'
              className='mt-8'
              register={forgotPasswordRegister}
              errorMessage={forgotPasswordErrors.email?.message}
            />
            <div className='mt-3'>
              <Button
                type='submit'
                className='w-full rounded-xl text-center bg-pink-300 py-4 px-2 uppercase text-white text-sm hover:bg-pink-600 flex justify-center items-center'
                isLoading={forgotPasswordMutation.isPending}
                disabled={forgotPasswordMutation.isPending}
              >
                Gửi
              </Button>
            </div>
            <div className='mt-3'>
              <button
                type='button'
                className='w-full text-center text-blue-500'
                onClick={() => {
                  setShowForgotPassword(false)
                  resetLoginForm() // Reset the login form
                }}
              >
                Quay lại
              </button>
            </div>
          </form>
        )}
      </div>
      <div>
        <div>
          <span>---------------------------</span>
          <span>hoặc</span>
          <span>---------------------------</span>
        </div>
        <div className='justify-center flex py-2'>
          <button
            className='bg-black text-white border-black border-2 w-[300px] rounded-lg justify-center items-center flex py-2 shadow-2xl hover:bg-white hover:text-black'
            onClick={signInGoogle}
          >
            <div className='pr-2'>
              <FontAwesomeIcon icon={faGoogle} />
            </div>
            <div>Google</div>
          </button>
        </div>
        <div className='my-4'>
          <div>
            <span className='text-gray-600 mr-1'>Bạn chưa có tài khoản? </span>
            <Link
              className='text-gray-500 underline hover:text-red-500'
              to={path.register}
            >
              Đăng Ký
            </Link>
          </div>
        </div>
      </div>
    </div>
  )
}
