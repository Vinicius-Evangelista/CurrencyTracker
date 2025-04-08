<template>
  <div class="container">
    <header class="header">
      <h1>💱 Currency Tracker</h1>
      <p>Track real-time exchange rates and keep a history</p>
    </header>

    <div class="card input-card">
      <form @submit.prevent="fetchRate" class="form">
        <div class="field">
          <label>From</label>
          <select v-model="fromCurrency">
            <option v-for="code in currencyCodes" :key="code">{{ code }}</option>
          </select>
        </div>

        <div class="field">
          <label>To</label>
          <select v-model="toCurrency">
            <option v-for="code in currencyCodes" :key="code">{{ code }}</option>
          </select>
        </div>

        <button type="submit" class="btn">Get Rate</button>
      </form>

      <div v-if="rate !== null" class="result">
        <p><strong>{{ fromCurrency }} ➜ {{ toCurrency }}</strong></p>
        <p class="rate">{{ rate }}</p>
      </div>
    </div>

    <div class="card history-card">
      <div class="history-header">
        <h2>Conversion History</h2>
        <input v-model="filterText" placeholder="Filter by currency..." class="filter-input" />
      </div>

      <ul class="history-list">
        <li v-for="(entry, index) in filteredHistory" :key="index" class="history-item">
        <div>
            <strong>{{ entry.from }} ➜ {{ entry.to }}</strong>
            <div class="sub">{{ entry.rate }}</div>
          </div>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue'

const fromCurrency = ref('USD')
const toCurrency = ref('EUR')
const rate = ref(null)
const filterText = ref('')
const history = ref([])

const currencyCodes = ['USD', 'EUR', 'BRL', 'GBP', 'JPY', 'ARS', 'CAD', 'AUD', 'CHF']

const fetchRate = async () => {
  rate.value = null
  const res = await fetch(`http://api:5000/api/currency-conversions`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      fromCurrency: fromCurrency.value,
      toCurrency: toCurrency.value
    })
  })

  if (res.ok) {
    const result = await res.json()
    rate.value = result.rate
    history.value.unshift({
      from: fromCurrency.value,
      to: toCurrency.value,
      rate: result.rate
    })
    
  } else {
    alert('Failed to fetch exchange rate')
  }
}

const fetchHistory = async () => {
  const res = await fetch(`http://api:5000/api/conversions?searchValue=${filterText.value}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json'
    }
  })
  if (res.ok) {
    const result = await res.json()
    history.value = result
  }
}

let debounceTimeout
watch(filterText, () => {
  clearTimeout(debounceTimeout)
  debounceTimeout = setTimeout(() => {
    fetchHistory()
  }, 400)
})

const filteredHistory = computed(() => {
  if (!filterText.value) return history.value
  const search = filterText.value.toLowerCase()
  return history.value.filter(
      entry =>
          entry.from.toLowerCase().includes(search) ||
          entry.to.toLowerCase().includes(search)
  )
})

onMounted(() => {
  fetchRate()
  fetchHistory()
})

</script>

<style scoped>
body {
  background: #1e1e1e;
  font-family: 'Segoe UI', sans-serif;
  color: #fff;
}

.container {
  max-width: 600px;
  margin: auto;
  padding: 2rem;
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.header {
  text-align: center;
  margin-bottom: 1rem;
}

.card {
  background: #2b2b2b;
  border-radius: 12px;
  padding: 2rem;
}

.input-card .form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.field label {
  display: block;
  margin-bottom: 0.25rem;
  font-weight: 500;
}

.field select {
  padding: 0.5rem;
  border: none;
  border-radius: 6px;
  background: #1e1e1e;
  color: #fff;
}

.btn {
  padding: 0.7rem;
  border: none;
  background: #2d88ff;
  color: white;
  font-weight: bold;
  border-radius: 6px;
  cursor: pointer;
  transition: background 0.3s;
}
.btn:hover {
  background: #1c6fe4;
}

.result {
  margin-top: 1.5rem;
  font-size: 1.2rem;
  text-align: center;
}
.result .rate {
  font-size: 2rem;
  font-weight: bold;
  color: #2d88ff;
}

.history-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  flex-wrap: wrap;
}

.history-header h2 {
  margin: 0;
  font-size: 1.25rem;
}

.filter-input {
  padding: 0.5rem;
  border: none;
  border-radius: 6px;
  background: #1e1e1e;
  color: white;
  min-width: 180px;
  flex-shrink: 1;
}


.history-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.history-item {
  padding: 1rem;
  border-bottom: 1px solid #444;
  display: flex;
  justify-content: space-between;
}

.sub {
  color: #aaa;
  font-size: 0.9rem;
  margin-top: 0.25rem;
}
</style>