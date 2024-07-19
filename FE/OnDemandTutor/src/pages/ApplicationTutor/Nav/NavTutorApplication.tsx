import { Link, useLocation } from 'react-router-dom'
import { path } from '../../../constant/path'
import { useEffect, useState } from 'react'

export default function NavStudentRequestList() {
  const location = useLocation()
  const [selected, setSelected] = useState(location.pathname)

  return (
    <div className='flex justify-around p-5 border-2 shadow-xl mb-5'>
      {/*   */}
      <Link to={path.tutorViewApplicationReject}>
        <div
          onClick={() => setSelected(path.tutorViewApplicationReject)}
          className={`text-base font-bold cursor-pointer py-1 relative after:absolute after:bottom-0 after:left-0 after:bg-pink-600 after:h-0.5
           ${
             selected === path.tutorViewApplicationReject
               ? 'text-pink-600 after:w-full'
               : 'hover:text-pink-600 after:w-0 hover:after:w-full'
           } after:transition-all after:ease-in-out after:duration-100`}
        >
          Đơn bị từ chối
        </div>
      </Link>

      <Link to={path.tutorViewApplicationSpending}>
        <div
          onClick={() => setSelected(path.tutorViewApplicationSpending)}
          className={`text-base font-bold cursor-pointer py-1 relative after:absolute after:bottom-0 after:left-0 after:bg-pink-600 after:h-0.5 ${
            selected === path.tutorViewApplicationSpending
              ? 'text-pink-600 after:w-full'
              : 'hover:text-pink-600 after:w-0 hover:after:w-full'
          } after:transition-all after:ease-in-out after:duration-100`}
        >
          Đơn chờ duyệt
        </div>
      </Link>

      <Link to={path.tutorViewApplication}>
        <div
          onClick={() => setSelected(path.tutorViewApplication)}
          className={`text-base font-bold cursor-pointer py-1 relative after:absolute after:bottom-0 after:left-0 after:bg-pink-600 after:h-0.5 ${
            selected === path.tutorViewApplication
              ? 'text-pink-600 after:w-full'
              : 'hover:text-pink-600 after:w-0 hover:after:w-full'
          } after:transition-all after:ease-in-out after:duration-100`}
        >
          Đơn đã duyệt
        </div>
      </Link>
    </div>
  )
}
