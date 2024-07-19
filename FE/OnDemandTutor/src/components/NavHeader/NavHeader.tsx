import { QueryClient, useMutation, useQueryClient } from '@tanstack/react-query'
import { useContext, useEffect } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { authApi } from '../../api/auth.api'

// import { getRefreshTokeNFromLS } from '../../utils/auth'
import userImage from '../../assets/img/user.svg'
import { path } from '../../constant/path'
import { roles } from '../../constant/roles'
import { AppContext } from '../../context/app.context'
import { clearLS, getProfileFromLS } from '../../utils/auth'
import Popover from '../Popover/Popover'
import UserButton from '../UserBotton'
import { LogoutReqBody } from '../../types/user.request.type'
import { User } from '../../types/user.type'

export default function NavHeader() {
  //const [count, setCount] = useState(0) // State để quản lý số lượng thông báo
  // const receiveNotification = () => {
  //   //  hàm để nhận thông báo mới, vd sử dụng button onclick bằng hàm này
  //   setCount(count + 1)
  // }

  const navigate = useNavigate()

  const queryClient = useQueryClient()

  const user: User = getProfileFromLS()

  const {
    isAuthenticated,
    setIsAuthenticated,
    refreshToken,
    profile,
    setProfile
  } = useContext(AppContext)

  const logoutMutation = useMutation({
    mutationFn: (body: LogoutReqBody) => authApi.logoutAccount(body)
  })

  const handleLogout = () => {
    console.log(222)

    logoutMutation.mutate(
      { refresh_token: refreshToken },
      {
        onSuccess: () => {
          setProfile(null)
          queryClient.removeQueries({ queryKey: ['Request'] })
          navigate(path.login)
          setIsAuthenticated(false)
        }
      }
    )
  }
  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('vi-VN', {
      style: 'currency',
      currency: 'VND'
    }).format(amount)
  }

  useEffect(() => {
    console.log('profile', profile)
  }, [profile])

  return (
    <div className='hidden md:flex justify-start  gap-5'>
      <UserButton
        isAuthenticated={isAuthenticated}
        profile={profile ? profile : user}
      />

      {isAuthenticated && (
        <Popover
          className='flex my-2 items-center hover:text-pink-400 cursor-pointer '
          renderPopover={
            <div className='shadow-md mt-2 rounded-sm border border-gray-200'>
              <Link
                to={path.user}
                className='block py-3 px-4 hover:bg-slate-100 bg-white hover:text-cyan-500 w-full text-left'
              >
                Tài khoản của tôi
              </Link>

              {profile?.roles.toLowerCase() === roles.moderator && (
                <Link
                  to={path.Moderator.mod}
                  className='block py-3 px-4 hover:bg-slate-100 bg-white hover:text-cyan-500 w-full text-left'
                >
                  Kiểm duyệt
                </Link>
              )}
              {profile?.roles.toLowerCase() === roles.admin && (
                <Link
                  to={path.Admin.admin}
                  className='block py-3 px-4 hover:bg-slate-100 bg-white hover:text-cyan-500 w-full text-left'
                >
                  Quản lý
                </Link>
              )}
              {(profile?.roles.toLowerCase() === roles.student ||
                profile?.roles.toLowerCase() === roles.tutor) && ( // phân đôi này ra
                <div>
                  <Link
                    to={path.deposit}
                    className='block py-3 px-4 hover:bg-slate-100 bg-white hover:text-cyan-500 w-full text-left'
                  >
                    Nạp tiền
                  </Link>
                  <Link
                    to={path.support}
                    className='block py-3 px-4 hover:bg-slate-100 bg-white hover:text-cyan-500 w-full text-left'
                  >
                    Hỗ trợ
                  </Link>

                  <Link
                    to={path.tutorViewApplicationSpending}
                    className='block py-3 px-4 hover:bg-slate-100 bg-white hover:text-cyan-500 w-full text-left'
                  >
                    Xem đơn của gia sư
                  </Link>
                  <Link
                    to={path.studentViewRequestList}
                    className='block py-3 px-4 hover:bg-slate-100 bg-white hover:text-cyan-500 w-full text-left'
                  >
                    Xem yêu cầu của bạn
                  </Link>

                  {profile.roles.toLowerCase() == roles.student ? (
                    <>
                      {/* <Link
                        to={path.bookedService}
                        className='block py-3 px-4 hover:bg-slate-100 bg-white hover:text-cyan-500 w-full text-left'
                      >
                        Dịch vụ đã đăng kí
                      </Link> */}
                      <Link
                        to={path.myClassLayout}
                        className='block py-3 px-4 hover:bg-slate-100 bg-white hover:text-cyan-500 w-full text-left'
                      >
                        Lớp học của bạn
                      </Link>
                    </>
                  ) : (
                    <>
                      <Link
                        to={path.tutorviewAllOwnService}
                        className='block py-3 px-4 hover:bg-slate-100 bg-white hover:text-cyan-500 w-full text-left'
                      >
                        Dịch vụ bạn đã tạo
                      </Link>
                      <Link
                        to={path.myService}
                        className='block py-3 px-4 hover:bg-slate-100 bg-white hover:text-cyan-500 w-full text-left'
                      >
                        Lớp học dịch vụ của bạn
                      </Link>
                    </>
                  )}
                </div>
              )}

              <button
                onClick={handleLogout}
                className='block py-3 px-4 hover:bg-slate-100 bg-white hover:text-cyan-500 w-full text-left'
              >
                Đăng xuất
              </button>
            </div>
          }
        >
          <div className='w-6 h-6 mr-2 ml-10 flex-shink-0 '>
            <img
              src={profile?.avatar ? profile.avatar : userImage}
              alt='avatar'
              className='h-full w-full rounded-full object-cover'
            />
          </div>

          <div className='text-black hover:text-pink-400'>
            {profile?.fullName}
          </div>
        </Popover>
      )}
      {!isAuthenticated && (
        <div className='flex items-end justify-end ml-[20rem]'>
          <Link
            to={path.login}
            className='transition duration-150 ease-in-out border-black border-2 px-2 py-2 rounded-lg hover:bg-gray-200 mr-1 '
          >
            Đăng Nhập
          </Link>
          <div className='border-r-[1px] border-r-white/40 h-4' />
          <Link
            to={path.register}
            className='bg-pink-500 border-black border-2 px-2 py-2 rounded-lg hover:bg-pink-400'
          >
            Đăng kí
          </Link>
        </div>
      )}
    </div>
  )
}
