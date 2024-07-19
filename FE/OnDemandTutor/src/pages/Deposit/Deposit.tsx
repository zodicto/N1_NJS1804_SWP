import { useMutation } from '@tanstack/react-query'
import React, { useContext, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import { paymentApi } from '../../api/payment.api'
import userApi from '../../api/user.api'
import { AppContext } from '../../context/app.context'
import { reqDeposit } from '../../types/user.request.type'

import { User } from '../../types/user.type'
import { getProfileFromLS } from '../../utils/auth'

const user: User = getProfileFromLS()
export default function Deposit() {
  const [amount, setAmount] = useState<number>(100000)

  const {
    isAuthenticated,
    setIsAuthenticated,
    refreshToken,
    profile,
    setProfile
  } = useContext(AppContext)

  const depositMutation = useMutation({
    mutationFn: (body: reqDeposit) => paymentApi.deposit(body)
  })

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()

    try {
      const formData: reqDeposit = {
        id: profile?.id ? profile.id : user.id,
        amount: amount
      }
      await depositMutation.mutateAsync(formData, {
        onSuccess: (data) => {
          if (data) {
            toast.success('Đang chuyển trang')
            window.location.href = data.data.data // Chuyển hướng đến URL từ response
          } else {
            alert('Có lỗi xảy ra, vui lòng thử lại.')
          }
        },
        onError: (error) => {
          console.error(error)
          alert('Có lỗi xảy ra, vui lòng thử lại.')
        }
      })
    } catch (error) {
      console.error('Error during deposit:', error)
      alert('Có lỗi xảy ra, vui lòng thử lại.')
    }
  }

  return (
    <div className='max-w-md mx-auto my-4 bg-white p-6 rounded-md shadow-md'>
      <h1 className='text-2xl font-bold mb-4'>Nạp Tiền</h1>
      <form onSubmit={handleSubmit} className='space-y-4'>
        {/* <div>
            <label htmlFor="fullName" className="block text-sm font-medium text-gray-700">
              Tên đầy đủ:
            </label>
            <input
              type="text"
              id="fullName"
              value={fullName}
              onChange={(e) => setFullName(e.target.value)}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-pink-500 focus:border-pink-500 sm:text-sm"
              required
            />
          </div> */}

        <div>
          <label
            htmlFor='amount'
            className='block text-sm font-medium text-gray-700'
          >
            Mệnh giá:
          </label>
          <select
            id='amount'
            value={amount}
            onChange={(e) => setAmount(parseInt(e.target.value))}
            className='mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-pink-500 focus:border-pink-500 sm:text-sm'
            required
          >
            <option value={10000}>10,000 VND</option>
            <option value={20000}>20,000 VND</option>
            <option value={50000}>50,000 VND</option>
            <option value={100000}>100,000 VND</option>
            <option value={200000}>200,000 VND</option>
            <option value={500000}>500,000 VND</option>
          </select>
        </div>

        <div>
          <button
            type='submit'
            className='w-full bg-pink-500 text-white py-2 px-4 rounded-md hover:bg-pink-600 focus:outline-none focus:ring-2 focus:ring-pink-500 focus:ring-opacity-50'
          >
            Nạp Tiền
          </button>
        </div>
      </form>
    </div>
  )
}

// NCB
// Thành công

// 9704198526191432198
// NGUYEN VAN A
// 07/15
// 123456
