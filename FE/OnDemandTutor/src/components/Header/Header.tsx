import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import aaa from '../../assets/img/aaaaaa.jpg'
import { path } from '../../constant/path'
import { roles } from '../../constant/roles'
import { User } from '../../types/user.type'
import { getProfileFromLS } from '../../utils/auth'
import NavHeader from '../NavHeader'
import Popover from '../Popover/Popover'

export default function Header() {
  const [user, setUser] = useState<User | null>(null)

  useEffect(() => {
    const userProfile = getProfileFromLS()
    if (userProfile) {
      setUser(userProfile)
    }
  }, [])

  return (
    <header className='container h-[8rem] bg-transparent w-full border-2 shadow-lg rounded-2xl mt-2 mb-5 hover:shadow-black hover:shadow-lg'>
      <div className='max-w-7xl mx-auto px-4'>
        <div className='grid grid-cols-12 gap-1 items-end'>
          <nav className='h-50 flex items-start col-span-12 md:col-span-3 pr-0 md:pr-20'>
            <Link to={path.home}>
              <div className='min-w-16  w-32 pt-[100%] mt-2 relative'>
                <div className='absolute top-0 left-0 w-[250px] h-full object-cover'>
                  <img src={aaa} alt='logo' />
                </div>
              </div>
            </Link>
          </nav>
          <div className='h-36 col-span-12 md:col-span-4 text-2xl  w-[35rem]'>
            <div className=' min-w-56 pr-0 md:pr-[50px] pt-4 md:pt-[40px] justify-around items-center flex flex-wrap'>
              <Link
                to={path.home}
                className='text-base font-bold cursor-pointer hover:text-pink-600 py-1 relative after:absolute after:bottom-0 after:left-0
                                    after:bg-pink-600 after:h-0.5 after:w-0 hover:after:w-full after:transition-all after:ease-in-out after:duration-100;'
              >
                Trang chủ
              </Link>
              {(user?.roles.toLowerCase() === roles.student ||
                user?.roles.toLowerCase() === roles.tutor) && (
                <React.Fragment>
                  <Popover
                    className='flex items-center py-1 cursor-pointer'
                    renderPopover={
                      <div className='rounded-3xl shadow-black shadow-xl'>
                        <div className='w-[25rem] flex mt-0.5 items-center justify-between text-center text-[10px] px-auto rounded-sm'>
                          <Link
                            to={path.registerAsTutor}
                            className='py-2 w-[15rem] h-full bg-pink-400 text-black rounded-l-3xl hover:text-white hover:bg-black hover:shadow-xl hover:shadow-white'
                          >
                            Đăng ký thành giảng viên
                          </Link>
                          <Link
                            to={path.sideBarMenu}
                            className='py-2 w-[15rem] h-full bg-pink-400 text-black rounded-r-3xl hover:text-white hover:bg-black hover:shadow-xl hover:shadow-white'
                          >
                            Danh sách lớp
                          </Link>
                        </div>
                      </div>
                    }
                  >
                    <div
                      className='mx-2 text-base font-bold cursor-pointer hover:text-pink-600 py-1 relative after:absolute after:bottom-0 after:left-0
                                    after:bg-pink-600 after:h-0.5 after:w-0 hover:after:w-full after:transition-all after:ease-in-out after:duration-100;'
                    >
                      Loại dịch vụ
                    </div>
                  </Popover>
                  <Link
                    to={path.tutorList}
                    className='mx-2 text-base font-bold cursor-pointer hover:text-pink-600 py-1 relative after:absolute after:bottom-0 after:left-0
                                    after:bg-pink-600 after:h-0.5 after:w-0 hover:after:w-full after:transition-all after:ease-in-out after:duration-100;'
                  >
                    Danh sách các gia sư
                  </Link>
                </React.Fragment>
              )}
              {(user?.roles.toLowerCase() === roles.moderator ||
                user?.roles.toLowerCase() === roles.admin) && (
                <React.Fragment>
                  <Link
                    to={path.sideBarMenu}
                    className=' text-sm font-bold cursor-pointer hover:text-pink-600 py-1 relative after:absolute after:bottom-0 after:left-0
                after:bg-pink-600 after:h-0.5 after:w-0 hover:after:w-full after:transition-all after:ease-in-out after:duration-100;'
                  >
                    Danh sách lớp
                  </Link>
                  <Link
                    to={path.tutorList}
                    className=' text-base font-bold cursor-pointer hover:text-pink-600 py-1 relative after:absolute after:bottom-0 after:left-0
                after:bg-pink-600 after:h-0.5 after:w-0 hover:after:w-full after:transition-all after:ease-in-out after:duration-100;'
                  >
                    Danh sách các gia sư
                  </Link>
                </React.Fragment>
              )}

              {!(
                user?.roles.toLowerCase() === roles.moderator ||
                user?.roles.toLowerCase() === roles.admin ||
                user?.roles.toLowerCase() === roles.student ||
                user?.roles.toLowerCase() === roles.tutor
              ) && (
                <div
                  className=' mt-1 text-base font-bold cursor-pointer hover:text-pink-600 py-1 relative after:absolute after:bottom-0 after:left-0
                                     '
                >
                  Các tính năng và hỗ trợ sẽ được kích hoạt ngay khi bạn đăng
                  nhập hoặc đăng ký
                </div>
              )}
            </div>
          </div>
          <nav className='h-36 pt-2 md:pt-[40px] col-span-12 md:col-span-5'>
            <NavHeader />
          </nav>
        </div>
      </div>
    </header>
  )
}
