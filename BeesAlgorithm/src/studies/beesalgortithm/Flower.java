package studies.beesalgortithm;

import java.util.Random;

public class Flower implements Comparable<Flower> {
	
	private int processorsMaxAmount = BeesAlgorithm.getProcessorsMaxAmount();
	private int nodeAmounts = BeesAlgorithm.getNodesAmount();
	private int solution[];
	private double solutionValue; 
	
	public Flower() {
		this.solution = new int[nodeAmounts]; 
	}

	public void choseFlower(){
		Random rn = new Random();
		for(int i=0;i<nodeAmounts;i++){
		solution[i] = rn.nextInt() % processorsMaxAmount;
		}
		computeSolutionValue();
	}
	
	public Flower choseFlowerNeighbor(){
		//TODO zaimplementowaæ to
		Flower flower = new Flower(); 
		flower.computeSolutionValue();
		return flower;
	}
	
	private void computeSolutionValue(){
		//TODO Implement this function
		// This is fake implementation only for test purposes 
		for(int i=0;i<nodeAmounts;i++){
			setSolutionValue(getSolutionValue() + solution[i]);
		}
	}

	public double getSolutionValue() {
		return solutionValue;
	}

	public void setSolutionValue(double solutionValue) {
		this.solutionValue = solutionValue;
	}

	@Override
	public int compareTo(Flower flower) {
		if(this.getSolutionValue() > flower.getSolutionValue()){
			return 1;	
		} 
		else if(this.getSolutionValue() == flower.getSolutionValue()){
			return 0;	
		} else{
			return -1;	
		}
	}
}
