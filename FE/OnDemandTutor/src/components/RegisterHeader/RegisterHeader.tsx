import { Link } from 'react-router-dom'
import BUMBUM from '../../assets/img/BUMBUM.png'
import Popover from '../Popover/Popover'
import { path } from '../../constant/path'

export default function RegisterHeader() {
  return (
    <header className='  h-[8rem] bg-transparent shadow-lg rounded-2xl '>
      <div className='max-w-7xl mx-auto px-4'>
        <div className='grid grid-cols-12 gap-4 items-end'>
          {/* Logo */}
          <nav className='h-50 flex items-start col-span-3 pr-20'>
            <Link to='/'>
              <div className='w-32 pt-[100%] mt-2 relative'>
                <div className='absolute top-0 left-0 w-[250px] h-full object-cover'>
                  <img src={BUMBUM} alt='logo' />
                </div>
              </div>
            </Link>
          </nav>
          {/* Mid */}
          <div className='h-36 col-span-6 text-2xl'>
            <div className='pr-[50px] pt-[40px] justify-around items-center flex'>
              <Link
                to='/'
                className='text-base font-bold cursor-pointer hover:text-pink-600 py-1 relative after:absolute after:bottom-0 after:left-0
                                    after:bg-pink-600 after:h-0.5 after:w-0 hover:after:w-full after:transition-all after:ease-in-out after:duration-100;'
              >
                Trang chủ
              </Link>

              <Popover
                className='flex items-center py-1  cursor-pointer shadow-red-700'
                renderPopover={
                  <div className='shadow-red-700'>
                    <div className='  w-[20rem] flex mt-0.5   items-center justify-between text-center  text-[10px]  px-auto rounded-3xl'>
                      <Link
                        to={path.registerAsTutor}
                        className='py-2 w-[10rem] h-full  bg-pink-400 border-pink-400 border-2 text-black  rounded-l-3xl   hover:text-white'
                      >
                        Đăng ký trở thành giảng viên
                      </Link>
                      <Link
                        to={path.tutors}
                        className='py-2 w-[10rem]  h-full  text-white  bg-black border-black border-2  rounded-r-3xl  hover:text-pink-400'
                      >
                        danh sách lớp
                      </Link>
                    </div>
                  </div>
                }
              >
                <div
                  className='text-base font-bold cursor-pointer hover:text-pink-600 py-1 relative after:absolute after:bottom-0 after:left-0
                                    after:bg-pink-600 after:h-0.5 after:w-0 hover:after:w-full after:transition-all after:ease-in-out after:duration-100;'
                >
                  Loại dịch vụ
                </div>
              </Popover>
              <Link
                to='/'
                className='text-base font-bold cursor-pointer hover:text-pink-600 py-1 relative after:absolute after:bottom-0 after:left-0
                                    after:bg-pink-600 after:h-0.5 after:w-0 hover:after:w-full after:transition-all after:ease-in-out after:duration-100;'
              >
                Hỗ trợ
              </Link>
            </div>
          </div>
          {/* Login */}
          <nav className='h-36 pt-[48px] col-span-3'>
            <Link
              to={path.login}
              className='transition duration-150 ease-in-out border-black border-2 px-2 py-2 rounded-lg hover:bg-gray-200 mr-1 '
            >
              Đăng Nhập
            </Link>
            <Link
              to={path.register}
              className='bg-pink-500 border-black border-2 px-2 py-2 rounded-lg hover:bg-pink-400'
            >
              Đăng kí
            </Link>
          </nav>
        </div>
      </div>
    </header>
  )
}
