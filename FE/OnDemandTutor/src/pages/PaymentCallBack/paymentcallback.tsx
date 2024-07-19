import { useMutation } from '@tanstack/react-query';
import React, { useEffect, useState, useMemo } from 'react';
import { useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';
import { paymentApi } from '../../api/payment.api';
import { getProfileFromLS } from '../../utils/auth';
import { User } from '../../types/user.type';

const user = getProfileFromLS();

const PaymentCallback: React.FC = () => {
  const location = useLocation();
  const [queryParams, setQueryParams] = useState<{ [key: string]: string }>({});
  const [isApiCalled, setIsApiCalled] = useState(false); // Thêm trạng thái để kiểm soát việc gọi API

  useEffect(() => {
    const searchParams = new URLSearchParams(location.search); // Móc url và phân tách thành object, sau đó set vào queryParams
    const params: { [key: string]: string } = {};
    searchParams.forEach((value, key) => {
      params[key] = value;
    });
    setQueryParams(params);
  }, [location.search]);

  const formatAmount = (amountString: string) => { // Hàm format lại số tiền nộp
    let amount = parseInt(amountString);
    amount = amount / 100; // Xử lý số dư hai số 0
    let formattedAmount = amount.toLocaleString('vi-VN');
    formattedAmount = formattedAmount.replace(/\./g, ',');
    formattedAmount += ' VNĐ';
    formattedAmount = formattedAmount.replace(/,\s*VNĐ$/, ' VNĐ');
    return formattedAmount;
  };

  const paymentCallback = useMutation({         
    mutationFn: (user: User) => paymentApi.paymentcallback(user),
    onSuccess: (data) => {
      toast.success('Payment callback success');
      console.log('Payment callback success:', data);
    },
    onError: (error) => {
      toast.error('Payment callback error');
      console.error('Payment callback error:', error);
    }
  });

  useEffect(() => {
    if (!isApiCalled) {
      paymentCallback.mutate(user);
      setIsApiCalled(true); // Đánh dấu API đã được gọi
    }
  }, [user, isApiCalled, paymentCallback]);

  return (
    <div className='bg-gray-100 border rounded-md shadow-md p-4 text-left'>
      <div>Đang đợi API bên bển</div>
      <h2 className='text-3xl font-bold text-center mb-5'>Nạp tiền thành công</h2>
      <div className='my-2'>Mã giao dịch: {queryParams.vnp_TransactionNo}</div>
      <div className='my-2'>Ngân hàng nguồn: {queryParams.vnp_BankCode}</div>
      <div className='my-2'>
        Tổng nạp tiền: <div className='font-bold text-red-400'>{formatAmount(queryParams.vnp_Amount)}</div>
      </div>
      <div className='my-2'>Lời nhắn:</div>
      <div className='my-2'>{queryParams.vnp_OrderInfo}</div>

      {/* Dùng Object.entries để render từng mục trong queryParams */}
      <ul className='my-2'>
        {/* {Object.entries(queryParams).map(([key, value]) => (
            <li key={key} className='mb-1'>
                <span className='font-bold'>{key}:</span> {value}
            </li>
        ))} */}
      </ul>
    </div>
  );
};

export default PaymentCallback;