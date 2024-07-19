import { Link } from 'react-router-dom'
import { path } from '../../constant/path'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faBell } from '@fortawesome/free-solid-svg-icons'
import { useState, useEffect, useRef } from 'react'
import { useMutation, useQuery } from '@tanstack/react-query'
import userApi from '../../api/user.api'
import { Notificate, User } from '../../types/user.type'
import { notifiReq } from '../../constant/notifi.req'
import { toast } from 'react-toastify'
import { roles } from '../../constant/roles'

interface UserButtonProps {
  isAuthenticated: boolean
  profile: User
}

const formatCurrency = (amount: number) => {
  return amount.toLocaleString('vi-VN', {
    style: 'currency',
    currency: 'VND'
  })
}

export default function ({ profile, isAuthenticated }: UserButtonProps) {
  // const [showNotificationWindow, setShowNotificationWindow] = useState(false)
  // const notificationRef = useRef<HTMLDivElement>(null)
  // const isFetchingRef = useRef(false) // Ref to track if refetch is in progress

  // const { data, refetch } = useQuery<Notificate[]>({
  //   queryKey: ['notification'],
  //   queryFn: () => userApi.getNotification(profile.id as string)
  // })

  // const updateNotification = useMutation({
  //   mutationFn: (id: string) => userApi.updateNotification(id),
  //   onSuccess: () => {
  //     // Only refetch if window is still open
  //     if (showNotificationWindow) {
  //       refetch()
  //     }
  //   },
  //   onError: (error) => {
  //     toast.error(error.message)
  //   }
  // })

  // const unreadNotifications =
  //   data?.filter(
  //     (notification) => notification.status.toLowerCase() === notifiReq.unRead
  //   ) || []

  // const hasNotification = unreadNotifications.length > 0

  // const handleClickOutside = (event: MouseEvent) => {
  //   if (
  //     notificationRef.current &&
  //     !notificationRef.current.contains(event.target as Node)
  //   ) {
  //     setShowNotificationWindow(false)
  //   }
  // }

  // useEffect(() => {
  //   // Fetch notifications when profile.id changes
  //   refetch()
  // }, [profile.id, refetch])

  // useEffect(() => {
  //   if (showNotificationWindow) {
  //     document.addEventListener('mousedown', handleClickOutside)
  //   } else {
  //     document.removeEventListener('mousedown', handleClickOutside)
  //   }
  //   return () => {
  //     document.removeEventListener('mousedown', handleClickOutside)
  //   }
  // }, [showNotificationWindow])

  // const toggleNotificationWindow = () => {
  //   setShowNotificationWindow(!showNotificationWindow)
  //   if (!showNotificationWindow && !isFetchingRef.current) {
  //     isFetchingRef.current = true
  //     unreadNotifications.forEach((notification) => {
  //       updateNotification.mutate(profile.id)
  //     })
  //     isFetchingRef.current = false
  //   }
  // }

  return (
    isAuthenticated && (
      <div className='flex ml-auto justify-end ml-90'>
        <div className='px-auto py-auto mt-2 mr-10 rounded-md justify-center items-center flex font-medium'>
          <Link to={path.deposit}>
            <span>Số dư:</span> {formatCurrency(profile?.accountBalance || 0)}
          </Link>
        </div>
      </div>
    )
  )
}

// {
//   profile.roles.toLowerCase() !== roles.moderator.toLowerCase() &&
//     profile.roles !== roles.admin && (
//       <div className='relative ml-3'>
//         {/* Bell icon with red dot */}
//         <FontAwesomeIcon
//           icon={faBell}
//           className='mt-3 cursor-pointer'
//           onClick={toggleNotificationWindow}
//         />
//         {hasNotification && (
//           <span className='absolute bottom-6 left-2 right-0 w-2 h-2 bg-red-600 rounded-full'></span>
//         )}
//         {showNotificationWindow && (
//           <div
//             ref={notificationRef}
//             className='absolute right-0 mt-8 w-64 bg-white shadow-lg rounded-lg border border-gray-200 z-50 overflow-y-auto max-h-72'
//           >
//             <div className='p-4'>
//               <h3 className='text-lg font-medium'>Thông báo</h3>
//               <ul>
//                 {unreadNotifications.length > 0 ? (
//                   unreadNotifications.map((notification, index) => (
//                     <li
//                       key={notification.idNotification}
//                       className='py-2 border-b border-gray-200 hover:text-pink-400'
//                     >
//                       {index + 1}. {notification.description}
//                     </li>
//                   ))
//                 ) : (
//                   <li className='py-2'>Không có thông báo mới</li>
//                 )}
//               </ul>
//             </div>
//           </div>
//         )}
//       </div>
//     )
// }
