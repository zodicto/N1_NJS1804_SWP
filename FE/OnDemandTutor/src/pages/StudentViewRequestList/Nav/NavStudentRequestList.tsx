import { Link, useLocation } from 'react-router-dom'
import { path } from '../../../constant/path'
import { useEffect, useState } from 'react'

export default function NavStudentRequestList() {
  const location = useLocation()
  const [selected, setSelected] = useState(location.pathname)

  return (
    <div className='flex justify-around p-5 border-2 shadow-xl mb-5'>
      {/*   */}
      <Link to={path.requestStudentReject}>
        <div
          onClick={() => setSelected(path.requestStudentReject)}
          className={`text-base font-bold cursor-pointer py-1 relative after:absolute after:bottom-0 after:left-0 after:bg-pink-600 after:h-0.5
           ${
             selected === path.requestStudentReject
               ? 'text-pink-600 after:w-full'
               : 'hover:text-pink-600 after:w-0 hover:after:w-full'
           } after:transition-all after:ease-in-out after:duration-100`}
        >
          Xem danh sách bị từ chối
        </div>
      </Link>

      <Link to={path.studentViewRequestList}>
        <div
          onClick={() => setSelected(path.studentViewRequestList)}
          className={`text-base font-bold cursor-pointer py-1 relative after:absolute after:bottom-0 after:left-0 after:bg-pink-600 after:h-0.5 ${
            selected === path.studentViewRequestList
              ? 'text-pink-600 after:w-full'
              : 'hover:text-pink-600 after:w-0 hover:after:w-full'
          } after:transition-all after:ease-in-out after:duration-100`}
        >
          Xem danh sách yêu cầu đang duyệt
        </div>
      </Link>

      <Link to={path.requestStudentCurrent}>
        <div
          onClick={() => setSelected(path.requestStudentCurrent)}
          className={`text-base font-bold cursor-pointer py-1 relative after:absolute after:bottom-0 after:left-0 after:bg-pink-600 after:h-0.5 ${
            selected === path.requestStudentCurrent
              ? 'text-pink-600 after:w-full'
              : 'hover:text-pink-600 after:w-0 hover:after:w-full'
          } after:transition-all after:ease-in-out after:duration-100`}
        >
          Xem danh sách yêu cầu đã duyệt
        </div>
      </Link>
    </div>
  )
}
