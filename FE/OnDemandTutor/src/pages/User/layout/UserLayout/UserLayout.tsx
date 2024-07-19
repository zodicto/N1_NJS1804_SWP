import { Outlet } from 'react-router-dom'
import UserSideNav from '../../components/UserSideNav'

export default function UserLayout() {
  return (
    <div className=' w-[85rem] bg-transparent py-16 px-auto  text-sm-gray-600 hover:bg-white hover:shadow-2xl hover:shadow-black rounded-3xl'>
      <div className='grid grid-cols-1 gap-6 md:grid-cols-12'>
        <div className='md:col-span-3 lg:col-span-2 mr-2'>
          <UserSideNav />
        </div>
        <div className='md:col-span-9 lg:col-span-10'>
          {/* khi mà xài outlet rồi là không có truyênf compoentn đc đâu nha */}
          <Outlet />
        </div>
      </div>
    </div>
  )
}
